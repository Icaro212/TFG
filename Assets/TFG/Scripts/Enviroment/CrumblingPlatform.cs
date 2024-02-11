using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{

    public Collider2D platformCollider;
    public bool playingAnim = false;
    private Animator anim;

    //Attach Objects
    [SerializeField] private List<Optional<GameObject>> optionalsObjects;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") & !playingAnim)
        {
            anim.SetTrigger("Crumble");
            foreach (Optional<GameObject> optional in optionalsObjects)
            {
                if (optional.Enabled)
                {
                    StartCoroutine(FaceOutOptional(optional.Value));
                }
            }
        }
        
    }

    private void Reforming()
    {
        anim.SetTrigger("Reform");
    }

    IEnumerator FaceOutOptional(GameObject optionalObject)
    {
        yield return null;
        optionalObject.GetComponent<Animator>().SetTrigger("FaceOut");
    }

}
