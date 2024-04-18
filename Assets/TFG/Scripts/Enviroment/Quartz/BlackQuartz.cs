using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackQuartz : MonoBehaviour
{

    private Animator anim;
    [SerializeField] private AudioClip quartzCollectedClip;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetTrigger("LevelCompleted");
        SoundFXManager.instance.PlaySoundFXClip(quartzCollectedClip, transform, 1f);
    }

    private void LoadMenu()
    {
        GameManager.instance.objectsInRoom.Clear();
        string currentLevel = GameManager.instance.LevelPlaying;
        GameManager.instance.levelsCompleted[currentLevel] = true;
        SaveSystem.SaveGame();
        SceneManager.LoadScene("MainMenu");
        foreach(LevelInfo card in GameManager.instance.infoCardsLevels)
        {
            card.UpdateScore();
        }
    }
}
