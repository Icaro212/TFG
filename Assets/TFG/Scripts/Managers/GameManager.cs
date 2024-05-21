using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

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
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip respawnClip;

    //LoadingLevel
    private AsyncOperation asyncLoadLevel;

    //GameData
    public string LevelPlaying;
    public Dictionary<string, int> numberOfDeathPerLevel;
    public Dictionary<string, Dictionary<string, bool>> quartzDictPerLevel;
    public Dictionary<string, bool> levelsCompleted;
    public int currentDeathsInLevel;
    public List<string> quartzCollected;

    //LevelInfoCards
    public List<LevelInfo> infoCardsLevels;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            numberOfDeathPerLevel = new Dictionary<string, int>();
            quartzDictPerLevel = new Dictionary<string, Dictionary<string, bool>>();
            levelsCompleted = new Dictionary<string, bool>();
            StartCoroutine(GameDataCheck());
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        objectsInRoom = new Dictionary<string, HashSet<GameObject>>();
    }

    public void Respawn() //Este Método se llama desde Unity en la Animacióngi
    {
        playerPosition.position = Vector3.zero;
        playerPosition.position = currentCheckPoint.position;
        playerDying = false;
        script.enabled = true;
        rb.gravityScale = 2.5f;
        anim.SetTrigger("respawn");
        SoundFXManager.instance.PlaySoundFXClip(respawnClip, transform, 1f);
        StartCoroutine(RestartingRoom(activeRoom));   
    }
    

    public void Die()
    {
        anim.SetTrigger("death");
        script.enabled = !script.enabled;
        rb.bodyType = RigidbodyType2D.Static;
        currentDeathsInLevel += 1;
        playerDying = true;
        col.isTrigger = true;
        SoundFXManager.instance.PlaySoundFXClip(deathClip, transform, 1f);
    }

    public IEnumerator RestartingRoom(string room)
    {
        yield return null;
        if (objectsInRoom.ContainsKey(room))
        {
            HashSet<GameObject> objects = objectsInRoom[room];
            foreach (GameObject obj in objects)
            {
                IRestartable elemt = obj.gameObject.GetComponent<IRestartable>();
                StartCoroutine(elemt.Restart());
            }
        }
        yield return new WaitForSeconds(0.7f);
        col.isTrigger = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public IEnumerator LoadingScene(string name)
    {
        GameManager.instance.LevelPlaying = name;
        asyncLoadLevel = SceneManager.LoadSceneAsync(name);
        yield return asyncLoadLevel;
    }

    private IEnumerator GameDataCheck()
    {
        List<string> levels = new List<string>();
        levels.Add("Tutorial");
        levels.Add("Level 1");
        levels.Add("Level 2");

        if (PlayerPrefs.GetInt("FirstTimePlaying", 1) == 1)
        {
            PlayerPrefs.SetInt("FirstTimePlaying", 0);
            for (int i = 0; i < levels.Count; i++)
            {
                string nameLevel = levels[i];
                numberOfDeathPerLevel.Add(nameLevel, 0);
                levelsCompleted.Add(nameLevel, false);

                Dictionary<string, bool> quartzDict = new Dictionary<string, bool>();
                AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(nameLevel, LoadSceneMode.Additive);
                yield return asyncLoadLevel;
                Quartz[] quartzInScene = Object.FindObjectsOfType<Quartz>();

                for (int j = 0; j < quartzInScene.Length; j++)
                {
                    quartzDict.Add(quartzInScene[j].gameObject.name, false);
                }

                AsyncOperation asyncUnloadLevel = SceneManager.UnloadSceneAsync(nameLevel);
                yield return asyncUnloadLevel;
                quartzDictPerLevel.Add(nameLevel, quartzDict);
            }
            SaveSystem.SaveGame();
        }
        else
        {
            GameData data = SaveSystem.LoadGame();
            for (int i = 0; i < levels.Count; i++)
            {
                string nameLevel = levels[i];
                numberOfDeathPerLevel.Add(nameLevel, data.numberOfDeathPerLevel[nameLevel]);
                levelsCompleted.Add(nameLevel, data.levelsCompleted[nameLevel]);
                quartzDictPerLevel.Add(nameLevel, data.quartzDictPerLevel[nameLevel]);

            }

        }       
    }

    public void SearchForPlayer()
    {
        GameObject playerInScene = Object.FindObjectOfType<PlayerMovement>().gameObject;
        playerPosition = playerInScene.GetComponent<Transform>();
        rb = playerInScene.GetComponent<Rigidbody2D>();
        col = playerInScene.GetComponent<Collider2D>();
        anim = playerInScene.GetComponent<Animator>();
        script = playerInScene.GetComponent<PlayerMovement>();
    }
}
