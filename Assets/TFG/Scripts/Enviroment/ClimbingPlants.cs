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
    public float intervalDeletion;

    public Transform segmentPrefab;
    private List<Transform> segmentsList = new List<Transform>();

    private Vector3 origPosition;
    private bool coroutineRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        bar = canvas.GetComponent<MagicBar>();
        origPosition = transform.position;
        segmentsList.Add(transform);
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
        bar.Cost(habilityCost);
        for(int i = 0; i < 3; i++)
        {
            Transform segment = Instantiate(segmentPrefab);
            segment.position = segmentsList[segmentsList.Count - 1].position;
            segmentsList.Add(segment);
            yield return null;
        }
        
        foreach(string direction in listOfDirections)
        {
            yield return new WaitForSeconds(interval);
            for (int i = segmentsList.Count - 1; i > 0; i--)
            {
                segmentsList[i].position = segmentsList[i - 1].position;
            }
            Vector2 aux = dictDirection[direction];
            Vector3 curentPosition = this.transform.position;
            this.transform.position = new Vector3(curentPosition.x + aux.x, curentPosition.y + aux.y, 0f);
        }

        for(int i = segmentsList.Count - 1; i > 0; i--)
        {
            Transform aux = segmentsList[i];
            segmentsList.RemoveAt(i);
            Destroy(aux.gameObject);
            yield return new WaitForSeconds(intervalDeletion);
        }

        transform.position = origPosition;
        coroutineRunning = false;
    }
}
