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


    private bool hasCollide;
    [SerializeField] private List<GameObject> areasOfCollision;

    private float timerCost = 0f;
    public float costInterval = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        bar = canvas.GetComponent<MagicBar>();
        playerRB = player.GetComponent<Rigidbody2D>();
        playerScript = player.GetComponent<PlayerMovement>();
        playerTransform = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasCollide && Input.GetButtonDown("Fire3") && bar.CheckValidityMovement(habilityCostPerSecond) && !playerScript.isWallClimbingHoriActive)
        {
            StartCoroutine(HorizontalClimb());
        }
    }

    private void FixedUpdate()
    {
        hasCollide = areasOfCollision.Any(area => area.GetComponent<AreaOfEffectWallHori>().isInArea);
    }

    private void Rotate(float z)
    {
        playerTransform.Rotate(0.0f, 0.0f, z);
    }


    private IEnumerator HorizontalClimb()
    {
        playerScript.isWallClimbingHoriActive = true;
        playerRB.velocity = Vector2.zero;
        Rotate(90f);
        while (bar.CheckValidityMovement(habilityCostPerSecond) && hasCollide)
        {
            float x = Input.GetAxis("Horizontal");
            float speedModifier = x > 0 ? .5f : 1;
            playerRB.velocity = new Vector2(x * (13 * speedModifier), playerRB.velocity.y);
            timerCost += Time.deltaTime;
            if (timerCost >= costInterval)
            {
                bar.Cost(habilityCostPerSecond);
                timerCost = 0f;
            }
            yield return null;
        }
        playerScript.isWallClimbingHoriActive = false;
        Rotate(270f);


    }
}