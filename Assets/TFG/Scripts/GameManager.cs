using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //Instance
    public static GameManager instance;

    //Restart objects when Dying
    public string activeRoom { set; get; }
    public Dictionary<string, HashSet<GameObject>> objectsInRoom { set; get; }

    //Player Death
    private Rigidbody2D rb;
    private Collider2D col;
    private Animator anim;
    private PlayerMovement script;
    public Transform currentCheckPoint;
    private Transform playerPosition;
    public bool playerDying { get; set; }



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerPosition = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        script = GetComponent<PlayerMovement>();
        objectsInRoom = new Dictionary<string, HashSet<GameObject>>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Respawn"))
        {
            currentCheckPoint = other.transform;
        }
    }

    private void Respawn() //Este Método se llama desde Unity en la Animación
    {
        playerPosition.position = Vector3.zero;
        playerPosition.position = currentCheckPoint.position;
        playerDying = false;
        script.enabled = true;
        rb.gravityScale = 2.5f;
        anim.SetTrigger("respawn");
        col.isTrigger = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        StartCoroutine(RestartingRoom(activeRoom));
    }

    public void Die()
    {
        anim.SetTrigger("death");
        script.enabled = !script.enabled;
        rb.bodyType = RigidbodyType2D.Static;
        playerDying = true;
        col.isTrigger = true;
    }

    public IEnumerator RestartingRoom(string room)
    {
        yield return null;
        if (objectsInRoom.ContainsKey(room))
        {
            HashSet<GameObject> objects = objectsInRoom[room];
            //Según se vaya ampliando con otros tipos podemos buscar una manera de acceder al método manualmente
            foreach (GameObject obj in objects)
            {
                Thwomp elemt = obj.gameObject.GetComponent<Thwomp>();
                StartCoroutine(elemt.Restart());
            }
        }
    }
}
