using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{

    public GameObject pauseMenu;
    private bool isPaused;
    
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isPaused) 
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if(SceneManager.GetActiveScene().name != "MainMenu")
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ReturnMenu()
    {
        ResumeGame();
        StartCoroutine(GameManager.instance.LoadingScene("MainMenu"));
    }

    public void RestartRoom()
    {
        ResumeGame();
        GameManager.instance.Die();
    }
}
