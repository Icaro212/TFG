using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public int numberOfDeath;
    public Dictionary<string, bool> quartzDict;
    public bool completed;

    public LevelData (int numberOfDeathAux, Dictionary<string, bool> quartzDictAux, bool completedAux)
    {
        numberOfDeath = numberOfDeathAux;
        quartzDict = quartzDictAux;
        completed = completedAux;
    }
}
