using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransaction : MonoBehaviour
{


    private bool isPlayerInArea;
    public GameObject Canvas;
    public string SceneDestination;
    
    // Update is called once per frame
    void Update()
    {
        if (isPlayerInArea && Input.GetButtonDown("Fire3"))
        {
            GoToScene();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        isPlayerInArea = true;
        Canvas.SetActive(true);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerInArea = false;
        Canvas.SetActive(false);
    }


    private void GoToScene()
    {
        SceneManager.LoadScene(SceneDestination);
    }
}
