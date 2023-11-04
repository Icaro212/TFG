using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject virtualCam;
    public GameObject room;
    private HashSet<GameObject> objectsInRoom;

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
            objectsInRoom.Add(other.gameObject);
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
