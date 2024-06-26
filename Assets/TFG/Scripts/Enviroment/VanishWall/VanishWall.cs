using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishWall : MonoBehaviour
{

    private Animator anim;
    private bool hasBeenDiscovered = false;
    [SerializeField] private AudioClip discoverClip;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !hasBeenDiscovered)
        {
            hasBeenDiscovered = true;
            anim.SetTrigger("Reveal");
            SoundFXManager.instance.PlaySoundFXClip(discoverClip, transform, 1f);
        }
    }

}
