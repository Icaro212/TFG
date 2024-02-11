using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    public GameObject objective;
    private Animator anim;
    private FollowList listContainingSelf;
    private bool isInList = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(objective != null)
        {
            Vector2 direction = objective.transform.position - transform.position;
            direction.Normalize();

            transform.Translate(direction *8* Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player") && !isInList)
        {
            listContainingSelf = collision.GetComponentInChildren<FollowList>();
            objective =listContainingSelf.AddFollower();
            isInList = true;
        }
    }

    public void DestroyKey()
    {
        anim.SetTrigger("hasBeenUsed");
        listContainingSelf.RemoveTarget(gameObject);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
