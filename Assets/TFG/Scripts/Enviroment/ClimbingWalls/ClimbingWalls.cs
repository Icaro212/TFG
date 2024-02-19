using System.Collections;
using System.Collections.Generic;
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

    private float timerCost = 0f;
    public float costInterval = 0.25f;

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
        if (hasCollide && (Input.GetButtonDown("Fire3") || playerScript.isWallClimbingActive) && bar.CheckValidityMovement(habilityCostPerSecond))
        {
            Debug.Log("Hey");
            playerScript.isWallClimbingActive = true;
            float y = Input.GetAxis("Vertical");
            float speedModifier = y > 0 ? .5f : 1;
            playerRB.velocity = new Vector2(playerRB.velocity.x, y * (13 * speedModifier));
            timerCost += Time.deltaTime;
            if (timerCost >= costInterval)
            {
                bar.Cost(habilityCostPerSecond);
                timerCost = 0f;
            }
        }
        else
        {
            playerScript.isWallClimbingActive = false;
        }
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

}
