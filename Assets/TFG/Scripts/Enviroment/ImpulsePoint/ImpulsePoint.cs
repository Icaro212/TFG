using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulsePoint : MonoBehaviour
{

    public GameObject player;
    private PlayerMovement playerScript;
    private Rigidbody2D playerRB;

    public GameObject canvas;
    public int habilityCost;
    private MagicBar bar;

    public Transform selfPosition;
    [SerializeField] private GameObject associatedRoom;
    private Collider2D col;
    private Vector2 impulse;
    private Vector3 originalPlayerPos;
    private bool playerInArea;
    private bool exitFlag = false;

    public Color disableColor;
    public Color waitingColor;
    private SpriteRenderer rend;
    // Start is called before the first frame update
    private void Start()
    {
        selfPosition = GetComponent<Transform>();
        bar = canvas.GetComponent<MagicBar>();
        playerRB = player.GetComponent<Rigidbody2D>();
        playerScript = player.GetComponent<PlayerMovement>();
        rend = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        col.enabled = associatedRoom.name.Equals(GameManager.instance.activeRoom);
        if (playerInArea && bar.CheckValidityMovement(habilityCost) && Input.GetButtonDown("Fire3"))
        {
            originalPlayerPos = player.transform.position;
            StartCoroutine(Impulse());
        }
    }

    private void FixedUpdate()
    {
        if (playerInArea)
        {
            float distantToCenter = Vector2.Distance(selfPosition.position, player.transform.position);
            float totalDistance = Vector2.Distance(selfPosition.position, originalPlayerPos);
            float percentaje = (totalDistance - distantToCenter) / distantToCenter; //Position in a line bettween the original position and the center of the GameObject given as a fraction of one
            switch (percentaje)
            {
                case < 0.5f:
                    CalculateImpulse(0.25f);
                    break;
                case < 0.75f:
                    CalculateImpulse(0.85f);
                    break;
                default:
                    CalculateImpulse(1);
                    break;
            }

        }
    }

    private void CalculateImpulse(float lerp)
    {
        float componentXAux = selfPosition.position.x - player.transform.position.x;
        float componentYAux = selfPosition.position.y - player.transform.position.y;
        float componentX = Mathf.Lerp(componentXAux, -componentXAux, lerp) ;
        float componentY = Mathf.Lerp(componentYAux, -componentYAux, lerp);
        impulse = new Vector2(componentX, componentY);
        
    }

    private IEnumerator Impulse()
    {
        playerScript.isImpulsePointAct = true;
        bar.Cost(habilityCost);
        yield return null;
        playerRB.velocity = Vector2.zero;
        while (Vector2.Distance(selfPosition.position, player.transform.position) >= 1)
        {
            playerScript.trailRenderer.emitting = true;
            playerRB.AddForce(impulse, ForceMode2D.Impulse);
            if (exitFlag) break;
            yield return null;
        }
        playerRB.velocity = Vector2.zero;
        playerScript.isImpulsePointAct = false;
        playerScript.trailRenderer.emitting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInArea = true;
            exitFlag = false;
            rend.color = waitingColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInArea = false;
            playerScript.isImpulsePointAct = false;
            exitFlag = true;
            rend.color = disableColor;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        float componentX = selfPosition.position.x - player.transform.position.x;
        float componentY = selfPosition.position.y - player.transform.position.y;
        Vector2 vector = new Vector2(componentX, componentY);
        Gizmos.DrawRay(selfPosition.position, vector*0.125f);
    }
}
