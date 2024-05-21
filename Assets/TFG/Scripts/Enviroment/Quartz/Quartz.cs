using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quartz : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer rend;
    [SerializeField] private AudioClip quartzCollectedClip;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        string levelPlaying = GameManager.instance.LevelPlaying;
        if (GameManager.instance.quartzDictPerLevel.ContainsKey(levelPlaying) && GameManager.instance.quartzDictPerLevel[levelPlaying][gameObject.name])
        {
            rend.color = new Color(0.4549f, 0.4549f, 0.4549f);
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("ObjectCollected");
            SoundFXManager.instance.PlaySoundFXClip(quartzCollectedClip, transform, 1f);
            GameManager.instance.quartzCollected.Add(gameObject.name);
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
