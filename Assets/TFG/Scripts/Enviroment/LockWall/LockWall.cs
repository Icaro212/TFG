using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockWall : MonoBehaviour
{

    [SerializeField] private int keys;
    public GameObject wall;
    
    private Animator anim;
    private Coroutine coroutineLock;
    public Collider2D col;
    private bool animationEnded;
    private enum NumberOfLock { None, OneLeft, TwoLeft }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Collider2D[] hitColliders = new Collider2D[1000];
            ContactFilter2D contactFilter = new ContactFilter2D();
            int colliderCount = Physics2D.OverlapCollider(col, contactFilter.NoFilter(), hitColliders);
            List<Collider2D> listOfKeys = new List<Collider2D>();
            for (int i = 0; i < colliderCount; i++)
            {
                GameObject elemnt = hitColliders[i].gameObject;
                if (elemnt.CompareTag("Key"))
                {
                    listOfKeys.Add(elemnt.GetComponent<Collider2D>());
                }
            }
            if (coroutineLock == null && listOfKeys.Count > 0)
            {
                coroutineLock = StartCoroutine(OpenLocks(listOfKeys));
            }
        }
    }

    private void DeactivatedWall()
    {
        wall.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void AnimationHasEnded()
    {
        animationEnded = true;
    }

    private IEnumerator OpenLocks(List<Collider2D> keysInArea)
    {
        foreach(Collider2D keyCol in keysInArea)
        {
            keys--;
            keyCol.GetComponent<Key>().DestroyKey();
            animationEnded = false;
            NumberOfLock state;
            switch (keys)
            {
                case 2:
                    state = NumberOfLock.TwoLeft;
                    break;
                case 1:
                    state = NumberOfLock.OneLeft;
                    break;
                default:
                    state = NumberOfLock.None;
                    break;
            }
            anim.SetInteger("LocksMissing", (int) state);
            yield return new WaitUntil(() => animationEnded);

        }
        coroutineLock = null;
    }


}
