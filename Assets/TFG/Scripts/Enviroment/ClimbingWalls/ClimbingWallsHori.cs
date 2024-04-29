using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClimbingWallsHori : MonoBehaviour
{
    public float habilityCostPerSecond;
    public GameObject canvas;
    private MagicBar bar;

    [SerializeField] private GameObject player;
    private Rigidbody2D playerRB;
    private Transform playerTransform;
    private PlayerMovement playerScript;

    private AreaOfEffectWallHori areaActivated;
    private Vector2 dirrectionOfCurrentWall;
    private bool hasBeenRotate;
    private bool hasCollide;
    [SerializeField] private List<GameObject> areasOfCollision;

    private float timerCost = 0f;
    public float costInterval = 0.25f;

    private Coroutine routineRunning;

    [SerializeField] private AudioClip climbClip;

    [SerializeField] private float extendedFrames = 5.0f;
    private bool buttonPressed = false;
    private int frameCounter = 0;

    private Vector2 dirrectionPlayer;
    // Start is called before the first frame update
    void Start()
    {
        bar = canvas.GetComponent<MagicBar>();
        playerRB = player.GetComponent<Rigidbody2D>();
        playerScript = player.GetComponent<PlayerMovement>();
        playerTransform = player.GetComponent<Transform>();
        hasBeenRotate = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire3") && routineRunning == null)
        {
            buttonPressed = true;
            frameCounter = 0;
            dirrectionPlayer = playerScript.data.orientation;
        }

        if (buttonPressed)
        {
            frameCounter++;
            if (frameCounter >= extendedFrames)
                buttonPressed = false;
        }

        if (hasCollide && buttonPressed && bar.CheckValidityMovement(habilityCostPerSecond) && !playerScript.isWallClimbingHoriActive)
        {
            areaActivated = areasOfCollision.Find(area => area.GetComponent<AreaOfEffectWallHori>().isInArea).gameObject.GetComponent<AreaOfEffectWallHori>();
            dirrectionOfCurrentWall = areaActivated.directionWall;
            routineRunning = StartCoroutine(HorizontalClimb());
        }
    }

    private void FixedUpdate()
    {
        hasCollide = areasOfCollision.Any(area => area.GetComponent<AreaOfEffectWallHori>().isInArea);
    }

    private void Rotate()
    {
        float angle = Vector2.Angle(dirrectionPlayer, dirrectionOfCurrentWall);
        if(angle == 90 || angle == 270)
        {
            hasBeenRotate = true;
            playerTransform.Rotate(0.0f, 0.0f, 90.0f);
        }
    }

    private void ReturnToOrigPos()
    {
        Debug.Log(hasBeenRotate);
        if (hasBeenRotate)
        {
            hasBeenRotate = false;
            playerTransform.Rotate(0.0f, 0.0f, 270.0f);
        }
    }


    private IEnumerator HorizontalClimb()
    {
        playerScript.isWallClimbingHoriActive = true;
        playerRB.velocity = Vector2.zero;
        Rotate();
        while (bar.CheckValidityMovement(habilityCostPerSecond) && hasCollide)
        {
            float x = Input.GetAxis("Horizontal");
            float speedModifier = x > 0 ? .5f : 1;
            playerRB.velocity = new Vector2(x * (13 * speedModifier), playerRB.velocity.y);
            timerCost += Time.deltaTime;
            if (timerCost >= costInterval)
            {
                bar.Cost(habilityCostPerSecond);
                if (x != 0)
                    SoundFXManager.instance.PlaySoundFXClip(climbClip, transform, 1f);
                timerCost = 0f;
            }
            if (routineRunning != null && Input.GetButtonDown("Fire3"))
            {
                break;
            }
            yield return null;
        }
        playerScript.isWallClimbingHoriActive = false;
        areaActivated = null;
        routineRunning = null;
        ReturnToOrigPos();


    }
}