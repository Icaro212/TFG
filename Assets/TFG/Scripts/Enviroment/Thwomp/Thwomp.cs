using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thwomp : MonoBehaviour
{
    //Original Position Stuff
    private Vector3 startPosition;
    public Transform thwompPosition;
    public Collider2D colliderThwomp;
    
    //RayCasting Stuff
    public Transform rayCastOrigin;
    public LayerMask playerMask;
    public LayerMask groundMask;
    
    //Checks
    private bool isPlayerUnder;
    private float groundDistance;

    //Movement
    public Rigidbody2D rb;
    public Vector2 force;

    //Killing
    [SerializeField]
    KillingArea killingArea;
    private bool isFalling;

    private void Start()
    {
        startPosition = thwompPosition.position;
    }

    private void Update()
    {
        Fall();
    }

    private void FixedUpdate()
    {
        CheckingConditions();
    }

    private void CheckingConditions()
    {
        isPlayerUnder = Physics2D.Raycast(rayCastOrigin.position, -Vector3.up, 15f, playerMask);
        groundDistance = Physics2D.Raycast(rayCastOrigin.position, -Vector3.up, 15f, groundMask).distance;
    }

    private void Fall()
    {
        if (isPlayerUnder && !isFalling)
        {
            isFalling = true;
            StartCoroutine(Falling());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isFalling = true;
            StartCoroutine(Falling());
        }
    }

    IEnumerator Falling()
    {
        while (groundDistance > 0.25 && isFalling)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(-force);
            if (killingArea.getPlayerIsInArea() && groundDistance > 1f)
            {
                GameManager.instance.Die();
                while (GameManager.instance.playerDying)
                {
                    yield return null;
                }
                StartCoroutine(Restart());
            }
            yield return null;
        }
    }

    public IEnumerator Restart()
    {
        isFalling = false;
        rb.velocity = Vector3.zero;
        rb.bodyType = RigidbodyType2D.Static;
        thwompPosition.position = startPosition;
        yield return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(rayCastOrigin.position, -Vector3.up*15);
    }
}
