using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{

    public Collider2D platformCollider;
    public bool playingAnim = false;
    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") & !playingAnim)
        {
            anim.SetTrigger("Crumble");
        }
        
    }

    private void Reforming()
    {
        anim.SetTrigger("Reform");
    }

}
