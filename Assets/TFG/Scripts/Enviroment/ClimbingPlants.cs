using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingPlants : MonoBehaviour, IRestartable
{

    public GameObject canvas;
    public int habilityCost;
    private MagicBar bar;

    private Dictionary<string, Vector2> dictDirection = new Dictionary<string, Vector2>();
    public List<string> listOfDirections;
    public float interval;
    public float intervalDeletion;

    public Transform segmentPrefab;
    private List<Transform> segmentsList = new List<Transform>();

    private Vector3 origPosition;
    private Coroutine coroutineMovementRunning;
    private Coroutine coroutineRestartRunning;
    public bool restartHappening = false;


    [SerializeField] private AudioClip bushClip;

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
        if(collision.gameObject.CompareTag("Player") && bar.CheckValidityMovement(habilityCost) && coroutineMovementRunning == null && coroutineRestartRunning == null)
        {
            coroutineMovementRunning = StartCoroutine(ClimbingPlant());
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
        }
        foreach(string direction in listOfDirections)
        {
            yield return new WaitForSeconds(interval);
            SoundFXManager.instance.PlaySoundFXClip(bushClip, transform, 1f);
            for (int i = segmentsList.Count - 1; i > 0; i--)
            {
                segmentsList[i].position = segmentsList[i - 1].position;
            }
            Vector2 aux = dictDirection[direction];
            Vector3 curentPosition = this.transform.position;
            this.transform.position = new Vector3(curentPosition.x + aux.x, curentPosition.y + aux.y, 0f);
        }
        coroutineMovementRunning = null;
        coroutineRestartRunning = StartCoroutine(Restart());
    }

    public IEnumerator Restart()
    {
        if (coroutineMovementRunning != null)
        {
            StopCoroutine(coroutineMovementRunning);
            coroutineMovementRunning = null;
        }
        if (!restartHappening)
        {
            restartHappening = true;
            for (int i = segmentsList.Count - 1; i > 0; i--)
            {
                int index = segmentsList.Count - 1;
                Transform aux = segmentsList[index];
                segmentsList.RemoveAt(index);
                Destroy(aux.gameObject);
                yield return new WaitForSeconds(intervalDeletion);
            }
            transform.position = origPosition;
            coroutineRestartRunning = null;
            restartHappening = false;
        }


    }
}
