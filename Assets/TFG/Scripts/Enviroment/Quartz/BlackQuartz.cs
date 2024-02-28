using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackQuartz : MonoBehaviour
{

    private Animator anim;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetTrigger("LevelCompleted");
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
