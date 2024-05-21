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

        // Blue Quartz Fragments
        Dictionary<string, bool> quartzFragmentsInLevel = GameManager.instance.quartzDictPerLevel[currentLevel];
        List<string> keys = new List<string>(quartzFragmentsInLevel.Keys);
        foreach (string keyaux in keys)
        {
            if (GameManager.instance.quartzCollected.Contains(keyaux))
            {
                quartzFragmentsInLevel[keyaux] = true;
            }
            
        }
        GameManager.instance.quartzDictPerLevel[currentLevel] = quartzFragmentsInLevel;

        //Black Quartz Fragments
        GameManager.instance.levelsCompleted[currentLevel] = true;

        //Death
        GameManager.instance.numberOfDeathPerLevel[currentLevel] += GameManager.instance.currentDeathsInLevel;
        SaveSystem.SaveGame();
        SceneManager.LoadScene("MainMenu");
        GameManager.instance.quartzCollected.Clear();
        GameManager.instance.currentDeathsInLevel = 0;
        foreach(LevelInfo card in GameManager.instance.infoCardsLevels)
        {
            card.UpdateScore();
        }
    }
}
