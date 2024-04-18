using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Dictionary<string, int> numberOfDeathPerLevel;
    public Dictionary<string, Dictionary<string, bool>> quartzDictPerLevel;
    public Dictionary<string, bool> levelsCompleted;


    public GameData (Dictionary<string, int> numberOfDeathPerLevelAux, Dictionary<string, Dictionary<string, bool>> quartzDictPerLevelAux, Dictionary<string, bool> levelsCompletedAux)
    {
        numberOfDeathPerLevel = numberOfDeathPerLevelAux;
        quartzDictPerLevel = quartzDictPerLevelAux;
        levelsCompleted = levelsCompletedAux;
    }
}
