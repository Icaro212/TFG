using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransaction : MonoBehaviour
{

    private bool isPlayerInArea;
    public GameObject canvas;
    public string sceneDestination;

    public string sceneOrigin;
    public bool playerHasEnter = false;
    public GameObject PlayerPrefav;
    public Vector3 playerPosition { get; set; }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInArea && Input.GetButtonDown("Fire3"))
        {
            StartCoroutine(GoToScene());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerPosition=collision.transform.position;
            isPlayerInArea = true;
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInArea = false;
            canvas.SetActive(false);
        }
    }

    private IEnumerator GoToScene()
    {
        DontDestroyOnLoad(this.gameObject);
        isPlayerInArea = false;
        canvas.SetActive(false);
        yield return null;
        SceneManager.LoadScene(sceneDestination);
        while (SceneManager.GetActiveScene().name == sceneOrigin)
        {
            yield return null;
        }
        playerHasEnter = true;
    }

}
