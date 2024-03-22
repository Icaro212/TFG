using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject virtualCam;
    public GameObject room;
    public GameObject canvas;
    private MagicBar bar;
    private Collider2D col;


    private void Start()
    {
        bar = canvas.GetComponent<MagicBar>();
        col = GetComponent<Collider2D>();
        Collider2D[] hitColliders = new Collider2D[1000];
        int colliderCount = Physics2D.OverlapCollider(col, new ContactFilter2D(), hitColliders);
        for (int i = 0; i < colliderCount; i++)
        {
            GameObject elemnt = hitColliders[i].gameObject;
            if (elemnt.CompareTag("ObjectReload"))
            {
                if (!GameManager.instance.objectsInRoom.ContainsKey(room.name))
                {
                    HashSet<GameObject> objectsInRoom = new HashSet<GameObject>();
                    objectsInRoom.Add(elemnt);
                    GameManager.instance.objectsInRoom.Add(room.name, objectsInRoom);
                }
                else
                {
                    GameManager.instance.objectsInRoom[room.name].Add(elemnt);
                }
            }
        }
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
