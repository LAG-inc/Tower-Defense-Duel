using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RpsLvl", menuName = "RpsCustom/RPSLvl", order = 0)]
public class LvlRps : ScriptableObject
{
    public int phasesInLvl;
    public float basePoints;
    public float waveFactor;
    public List<GenerableData> enemiesAvailableStage1 = new List<GenerableData>();
    public List<GenerableData> enemiesAvailableStage2 = new List<GenerableData>();
    public List<GenerableData> enemiesAvailableStage3 = new List<GenerableData>();
}