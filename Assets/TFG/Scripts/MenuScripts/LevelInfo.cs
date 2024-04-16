using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfo : MonoBehaviour
{

    public string levelName;

    public TextMeshProUGUI quartzCollected;
    public TextMeshProUGUI levelCompleted;
    public TextMeshProUGUI deaths;

    private void Start()
    {
        quartzCollected.text = getQuartz(levelName);
        levelCompleted.text = getLevel(levelName);
        deaths.text = getDeaths(levelName);
    }

    public void UpdateScore()
    {
        quartzCollected.text = getQuartz(levelName);
        levelCompleted.text = getLevel(levelName);
        deaths.text = getDeaths(levelName);
    }

    private string getQuartz(string levelNameAux)
    {
        Dictionary<string, bool>.ValueCollection values = GameManager.instance.quartzDictPerLevel[levelNameAux].Values;
        float totalValues = values.Count;
        float counter = 0;
        foreach (bool val in values)
        {
            if (val)
                counter++;
        }
        string result = counter.ToString() + "/" + totalValues.ToString();
        return result;
    }

    private string getLevel(string levelNameAux)
    {
        bool completed = GameManager.instance.levelsCompleted[levelNameAux];
        string result = completed ? "1" : "0" + "/1";
        return result;
    }

    private string getDeaths(string levelNameAux)
    {
        int deaths = GameManager.instance.numberOfDeathPerLevel[levelNameAux];
        string result = deaths > 999 ? "999" : deaths.ToString("000");
        return result;
    }
}
