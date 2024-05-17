using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingPlatform : MonoBehaviour, IRestartable
{

    private Vector3 origPosition;
    
    private Coroutine sinkingCourutine;
    private Coroutine goingUpCorutine;
    private bool playerInPlatform;

    [SerializeField] private Vector2 directionOfSink;
    [SerializeField] private float intesity;
    [SerializeField] private float interval;
    [SerializeField] private AudioClip sinkingClip;
    
    // Start is called before the first frame update
    void Start()
    {
        origPosition = transform.position;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && sinkingCourutine == null)
        {
            playerInPlatform = true;
            other.transform.SetParent(this.transform);
            if(goingUpCorutine != null)
            {
                StopCoroutine(goingUpCorutine);
                goingUpCorutine = null;
            }
            sinkingCourutine = StartCoroutine(Sinking());
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInPlatform = false;
            other.transform.SetParent(null);
        }
    }

    private IEnumerator Sinking()
    {
        while (playerInPlatform)
        {
            yield return new WaitForSeconds(interval);
            SoundFXManager.instance.PlaySoundFXClip(sinkingClip, transform, 1f);
            transform.Translate(directionOfSink * intesity * Time.fixedDeltaTime);
        }
        sinkingCourutine = null;
        goingUpCorutine = StartCoroutine(GoingUp());
    }

    private IEnumerator GoingUp()
    {
        while (Vector2.Distance(transform.position, origPosition)>0.01f)
        {
            yield return new WaitForSeconds(interval);
            SoundFXManager.instance.PlaySoundFXClip(sinkingClip, transform, 1f);
            transform.position = Vector2.MoveTowards(transform.position, origPosition, 1.5f*intesity*Time.fixedDeltaTime);
        }
        goingUpCorutine = null;
    }

    public IEnumerator Restart()
    {
        if (sinkingCourutine != null)
            StopCoroutine(sinkingCourutine);
        if (goingUpCorutine != null)
            StopCoroutine(goingUpCorutine);
        transform.position = origPosition;
        yield return null;
    }
}
