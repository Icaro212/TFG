using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockWall : MonoBehaviour
{

    [SerializeField] private int keys;
    public GameObject wall;
    private Coroutine coroutineLock;
    private bool isOpening = false;
    [SerializeField] private GameObject areaSearch;

    private Animator anim;
    private bool animationEnded;
    private enum NumberOfLock { None, OneLeft, TwoLeft }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && coroutineLock == null && !isOpening)
        {
            coroutineLock = StartCoroutine(OpenLocks());
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

    private IEnumerator OpenLocks()
    {
        isOpening = true;
        List<Collider2D> listOfKeys = areaSearch.GetComponent<AreaOfSearch>().SearchAllKeys();
        if(listOfKeys.Count > 0)
        {
            foreach (Collider2D keyCol in listOfKeys)
            {
                keyCol.GetComponent<Key>().DestroyKey();
                animationEnded = false;
                keys--;
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
                anim.SetInteger("LocksMissing", (int)state);
                yield return new WaitUntil(() => animationEnded);
            }
        }
        coroutineLock = null;
        isOpening = false;
    }


}
