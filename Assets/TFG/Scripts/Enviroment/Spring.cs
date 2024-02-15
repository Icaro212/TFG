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
    
    [SerializeField] private GameObject player;
    private Rigidbody2D playerRB;
    private PlayerMovement playerScript;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = 2.0f;
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
            playerRB.velocity = Vector2.zero;
        }
    }

    private IEnumerator ReleaseCourutine()
    {
        bar.Cost(habilityCost);
        playerRB.AddForce(dirrection * potency, ForceMode2D.Impulse);
        while (Vector2.Distance(transform.position, player.transform.position) <= 7.5)
        {
            yield return null;
        }
        playerScript.isSpringActive = false;


    }

    private void SetSpringWaiting()
    {
        anim.SetBool("Release", false);
    }

    private void Release()
    {
        StartCoroutine(ReleaseCourutine());
    }

}
