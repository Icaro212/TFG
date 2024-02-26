using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quartz : MonoBehaviour
{

    public GameObject objective;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("ObjectCollected");
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
