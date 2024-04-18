using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    public GameObject objective;

    private FollowList listContainingSelf;
    private bool isInList = false;

    private Animator anim;

    [SerializeField] private AudioClip collectKeyClip;
    [SerializeField] private AudioClip destroyKeyClip;

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
            objective = listContainingSelf.AddFollower();
            SoundFXManager.instance.PlaySoundFXClip(collectKeyClip, transform, 1f);
            isInList = true;
        }
    }

    public void DestroyKey()
    {
        anim.SetTrigger("hasBeenUsed");
        SoundFXManager.instance.PlaySoundFXClip(destroyKeyClip, transform, 1f);
        listContainingSelf.RemoveTarget(objective);
        listContainingSelf.UpdateTargetsPosition();
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
