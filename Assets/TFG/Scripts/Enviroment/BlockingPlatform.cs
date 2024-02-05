using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingPlatform : MonoBehaviour
{

    private Animator anim;
    private Collider2D col;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.transform.position.x > transform.position.x)
        {
            col.isTrigger = false;
            anim.SetTrigger("Block the Way");
        }
    }

}
