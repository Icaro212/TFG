using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void Quit()
    {
        Debug.Log("Salir....");
        Application.Quit();
    }

    public void LoadScene(string name)
    {
        StartCoroutine(GameManager.instance.LoadingScene(name));
    }
}
