using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject virtualCam;
    public GameObject room;
    public GameObject canvas;
    private MagicBar bar;


    private void Start()
    {
        bar = canvas.GetComponent<MagicBar>();
    }


    //Player enter and exits room and thus the camaras change 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(true);
            GameManager.instance.activeRoom = room.name;
            bar.Reset();
        }

        if (other.CompareTag("ObjectReload"))
        {
            if (!GameManager.instance.objectsInRoom.ContainsKey(room.name))
            {
                HashSet<GameObject> objectsInRoom = new HashSet<GameObject>();
                objectsInRoom.Add(other.gameObject);
                GameManager.instance.objectsInRoom.Add(room.name, objectsInRoom);
            }
            else
            {
                GameManager.instance.objectsInRoom[room.name].Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(false);
            StartCoroutine(GameManager.instance.RestartingRoom(room.name));
        }
    }

}
