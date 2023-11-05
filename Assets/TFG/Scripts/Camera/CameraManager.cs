using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject virtualCam;
    public GameObject room;


    //Player enter and exits room and thus the camaras change 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(true);
            GameManager.instance.activeRoom = room.name;
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
                HashSet<GameObject> objectsInRoom = GameManager.instance.objectsInRoom[room.name];
                objectsInRoom.Add(other.gameObject);
                GameManager.instance.objectsInRoom.Add(room.name, objectsInRoom);
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
