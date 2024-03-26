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

    private Animator anim;

    private LineRenderer lineRend;

    // Start is called before the first frame update
    private void Start()
    {
        selfPosition = GetComponent<Transform>();
        bar = canvas.GetComponent<MagicBar>();
        playerRB = player.GetComponent<Rigidbody2D>();
        playerScript = player.GetComponent<PlayerMovement>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        lineRend = GetComponent<LineRenderer>();
        lineRend.enabled = false;
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
        if (lineRend.enabled)
        {
            float componentX = player.transform.position.x - selfPosition.position.x;
            float componentY = player.transform.position.y - selfPosition.position.y;
            Vector3 vector = new Vector3(componentX, componentY, 0f);
            lineRend.SetPosition(1, vector*0.7f);
        }
    }

    private void CalculateImpulse()
    {
        float distantToCenter = Vector2.Distance(selfPosition.position, player.transform.position);
        float totalDistance = Vector2.Distance(selfPosition.position, originalPlayerPos);
        float percentaje = 1 - (totalDistance - distantToCenter) / totalDistance;//Position in a line bettween the original position and the center of the GameObject given as a fraction of one
        float componentX = (selfPosition.position.x - player.transform.position.x)*percentaje;
        float componentY = (selfPosition.position.y - player.transform.position.y)*percentaje;
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
            CalculateImpulse();
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
            anim.SetBool("PlayerInArea", true);
            playerInArea = true;
            lineRend.enabled = true;
            exitFlag = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("PlayerInArea", false);
            playerInArea = false;
            lineRend.enabled = false;
            playerScript.isImpulsePointAct = false;
            exitFlag = true;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        float componentX = player.transform.position.x - selfPosition.position.x;
        //float componentX = selfPosition.position.x - player.transform.position.x;
        float componentY = player.transform.position.y - selfPosition.position.y;
        //float componentY = selfPosition.position.y - player.transform.position.y;
        Vector2 vector = new Vector2(componentX, componentY);
        Gizmos.DrawRay(selfPosition.position, vector*0.125f);
    }
}
