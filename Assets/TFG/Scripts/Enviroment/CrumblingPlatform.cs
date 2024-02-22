using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{

    public Collider2D platformCollider;
    public bool playingAnim = false;
    private Animator anim;

    //Attach Objects
    public List<Optional<GameObject>> optionalsObjects;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") & !playingAnim)
        {
            Crumble();
        }
        
    }

    private void Reforming()
    {
        anim.SetTrigger("Reform");
    }


    public void Crumble()
    {
        anim.SetTrigger("Crumble");
        foreach (Optional<GameObject> optional in optionalsObjects)
        {
            if (optional.Enabled)
            {
                optional.Value.GetComponent<Animator>().SetTrigger("FaceOut");
            }
        }
    }
    
    
    public IEnumerator CrumbleRoutine()
    {
        yield return null;
    }

}
