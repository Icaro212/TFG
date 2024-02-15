using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockWall : MonoBehaviour
{

    [SerializeField] private int keys;
    public GameObject wall;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    { 
        if(keys <= 0)
        {
            wall.GetComponent<BoxCollider2D>().isTrigger = true;
            anim.SetTrigger("Openning");

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Key"))
        {
            keys--;
            collision.GetComponent<Key>().DestroyKey();
        }
    }
}
