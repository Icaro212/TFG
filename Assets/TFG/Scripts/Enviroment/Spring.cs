using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public int potency;
    public Vector2 dirrection;

    public int habilityCost;
    public GameObject canvas;
    private MagicBar bar;

    private Animator anim;
    [SerializeField] private AudioClip springClip;
    
    [SerializeField] private GameObject player;
    private Rigidbody2D playerRB;
    private PlayerMovement playerScript;

    private bool playerRemains;
    public Optional<GameObject> crumblingGround;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = 3.0f;
        bar = canvas.GetComponent<MagicBar>();
        playerRB = player.GetComponent<Rigidbody2D>();
        playerScript = player.GetComponent<PlayerMovement>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && bar.CheckValidityMovement(habilityCost))
        {
            anim.SetBool("Release", true);
            playerScript.isSpringActive = true;
            playerRemains = true;
            playerRB.velocity = Vector2.zero;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRemains = false;
            SetSpringWaiting();
        }
    }

    private IEnumerator ReleaseCourutine()
    {
        if (playerRemains)
        {
            bar.Cost(habilityCost);
            playerRB.velocity = Vector2.zero;
            SoundFXManager.instance.PlaySoundFXClip(springClip, transform, 1f);
            playerRB.AddForce(dirrection * potency, ForceMode2D.Impulse);
            while (Vector2.Distance(transform.position, player.transform.position) <= 6.5)
            {
                yield return null;
            }
        }
        playerScript.isSpringActive = false;
        if(crumblingGround.Enabled)
        {
            anim.SetTrigger("FaceOut");
            crumblingGround.Value.GetComponent<Animator>().SetTrigger("Crumble");
        }
    }

    private void SetSpringWaiting()
    {
        anim.SetBool("Release", false);
    }

    private void Release()
    {
        StartCoroutine(ReleaseCourutine());
    }

    private void FaceIn()
    {
        anim.SetTrigger("FaceIn");
    }

    private void animVelocity(float x)
    {
        anim.speed = x;
    }

}
