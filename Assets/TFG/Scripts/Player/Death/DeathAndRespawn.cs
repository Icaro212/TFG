using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAndRespawn : MonoBehaviour
{
    //Death Consecuences
    public static DeathAndRespawn instance;
    public bool playerDying { get; set; }

//Managing Death
private Rigidbody2D rb;
    private Collider2D col;
    private Animator anim;
    private PlayerMovement script;
    public Transform currentCheckPoint;
    private Transform playerPosition;




    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerPosition = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        script = GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Respawn"))
        {
            currentCheckPoint = other.transform;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Spikes"))
        {
            Die();
        }
    }

    private void Respawn()
    {
        playerPosition.position = Vector3.zero;
        playerPosition.position = currentCheckPoint.position;
        playerDying = false;
        script.enabled = true;
        rb.gravityScale = 2.5f;
        anim.SetTrigger("respawn");
        col.isTrigger = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void Die()
    {
        anim.SetTrigger("death");
        script.enabled = !script.enabled;
        rb.bodyType = RigidbodyType2D.Static;
        playerDying = true;
        col.isTrigger = true;
    }
}
