using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClimbingWalls : MonoBehaviour
{
    public float habilityCostPerSecond;
    public GameObject canvas;
    private MagicBar bar;

    [SerializeField] private GameObject player;
    private Rigidbody2D playerRB;
    private PlayerMovement playerScript;

    private bool hasCollide;
    [SerializeField] private List<GameObject> areasOfCollision;

    private float timerCost = 0f;
    public float costInterval = 0.25f;

    private Coroutine routineRunning;

    [SerializeField] private AudioClip climbClip;

    [SerializeField] private float extendedFrames = 5.0f;
    private bool buttonPressed = false;
    private int frameCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        bar = canvas.GetComponent<MagicBar>();
        playerRB = player.GetComponent<Rigidbody2D>();
        playerScript = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire3") && routineRunning == null)
        {
            buttonPressed = true;
            frameCounter = 0;
        }

        if (buttonPressed)
        {
            frameCounter++;
            if (frameCounter >= extendedFrames)
                buttonPressed = false;
        }

        if (hasCollide && buttonPressed && bar.CheckValidityMovement(habilityCostPerSecond) && !playerScript.isWallClimbingActive)
        {
            routineRunning = StartCoroutine(VerticalClimb());
        }
    }

    private void FixedUpdate()
    {
        hasCollide = areasOfCollision.Any(area => area.GetComponent<AreaOfEffectWallHori>().isInArea);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hasCollide = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hasCollide = false;
        }
    }

    private IEnumerator VerticalClimb()
    {
        playerScript.isWallClimbingActive = true;
        while(bar.CheckValidityMovement(habilityCostPerSecond) && hasCollide)
        {
            float y = Input.GetAxis("Vertical");
            float speedModifier = y > 0 ? .5f : 1;
            playerRB.velocity = new Vector2(playerRB.velocity.x, y * (13 * speedModifier));
            timerCost += Time.deltaTime;
            if (timerCost >= costInterval)
            {
                bar.Cost(habilityCostPerSecond);
                if (y != 0)
                    SoundFXManager.instance.PlaySoundFXClip(climbClip, transform, 1f);
                timerCost = 0f;
            }
            if (routineRunning != null && Input.GetButtonDown("Fire3"))
            {
                break;
            }
            yield return null;
        }
        playerScript.isWallClimbingActive = false;
    }

}
