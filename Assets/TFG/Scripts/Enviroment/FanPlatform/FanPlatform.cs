using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanPlatform : MonoBehaviour
{

    [SerializeField] private int habilityCost;
    [SerializeField] private GameObject canvas;
    private MagicBar bar;

    [SerializeField] private GameObject player;
    private Rigidbody2D playerRB;
    private PlayerMovement playerScript;
    private Transform playerTransform;

    [SerializeField] private Vector2 directionOfWind;
    [SerializeField] private AreaOfEffect area;
    [SerializeField] private Transform heighTransform;
    [SerializeField] private float potency;
    [SerializeField] private float maxForceMagnitude;

    private Coroutine fanRunning;

    private float timerCost = 0f;
    [SerializeField] public float costInterval = 0.25f;
    private float timerAudio = 0f;
    private bool firstTime = true;
    private GameObject audioPlayerSFX;


    [SerializeField] private GameObject particleSystemWind;
    [SerializeField] private AudioClip fanClip;

    void Start()
    {
        bar = canvas.GetComponent<MagicBar>();
        playerRB = player.GetComponent<Rigidbody2D>();
        playerScript = player.GetComponent<PlayerMovement>();
        playerTransform = player.GetComponent<Transform>();
        timerAudio = fanClip.length;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {   
        if(other.gameObject.CompareTag("Player") && bar.CheckValidityMovement(habilityCost)  && fanRunning == null)
        {
            playerScript.isFanActivated = true;
            playerRB.velocity = Vector2.zero;
            playerRB.gravityScale = 2.75f;
            particleSystemWind.SetActive(true);
            fanRunning = StartCoroutine(WindActivate());
        }
    }


    private IEnumerator WindActivate()
    {
        while (area.playerInArea && bar.CheckValidityMovement(habilityCost))
        {
            if (playerScript.isWallClimbingHoriActive || playerScript.isWallClimbingActive)
                break;
            float distanceToHeight = playerScript.data.floorInXAixs ? heighTransform.position.y - playerTransform.position.y : heighTransform.position.x - playerTransform.position.x;
            distanceToHeight = playerScript.data.JumpPositive ? distanceToHeight : -distanceToHeight;
            if (Mathf.Abs(distanceToHeight) > 0.5f)
            {
                if (distanceToHeight > 0)
                {
                    Vector3 forceDirection = (directionOfWind.normalized * potency) / Mathf.Abs(distanceToHeight);
                    playerRB.AddForce(Vector3.ClampMagnitude(forceDirection, maxForceMagnitude), ForceMode2D.Impulse);
                }
                else
                {
                    Vector3 forceDirection = (directionOfWind.normalized * -potency * 0.85f) / Mathf.Abs(distanceToHeight);
                    playerRB.AddForce(Vector3.ClampMagnitude(forceDirection, maxForceMagnitude), ForceMode2D.Impulse);
                }

            }

            if (firstTime)
            {
                audioPlayerSFX = SoundFXManager.instance.PlaySoundFXClipAudio(fanClip, transform, 1f);
                firstTime = false;
            }

            if(timerAudio <= 0 || firstTime)
            {
                Destroy(audioPlayerSFX);
                audioPlayerSFX = SoundFXManager.instance.PlaySoundFXClipAudio(fanClip, transform, 1f);
                timerAudio = fanClip.length;
            }
            timerAudio -= Time.deltaTime;

            timerCost += Time.deltaTime;
            if (timerCost >= costInterval)
            {
                bar.Cost(habilityCost);
                timerCost = 0f;
            }
            yield return null;
        }
        Destroy(audioPlayerSFX);
        particleSystemWind.SetActive(false);
        playerScript.isFanActivated = false;
        firstTime = true;
        fanRunning = null;
    }
}
