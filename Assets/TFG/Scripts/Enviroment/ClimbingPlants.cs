using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingPlants : MonoBehaviour
{

    public GameObject canvas;
    public int habilityCost;
    private MagicBar bar;

    private static Dictionary<string, Vector2> dictDirection = new Dictionary<string, Vector2>();
    public List<string> listOfDirections;
    public float interval;

    private List<Transform> segments = new List<Transform>();

    private bool coroutineRunning = false;


    // Start is called before the first frame update
    void Start()
    {
        bar = canvas.GetComponent<MagicBar>();
        dictDirection.Add("Up", Vector2.up);
        dictDirection.Add("Right", Vector2.right);
        dictDirection.Add("Down", Vector2.down);
        dictDirection.Add("Left", Vector2.left);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && bar.CheckValidityMovement(habilityCost) && !coroutineRunning)
        {
            coroutineRunning = true;
            StartCoroutine(ClimbingPlant());
        }
    }

    private IEnumerator ClimbingPlant()
    {
        foreach(string direction in listOfDirections)
        {
            yield return new WaitForSeconds(interval);
            Vector2 aux = dictDirection[direction];
            Vector3 curentPosition = this.transform.position;
            this.transform.position = new Vector3(curentPosition.x + aux.x, curentPosition.y + aux.y, 0f);
        }
        coroutineRunning = false;
    }
}
