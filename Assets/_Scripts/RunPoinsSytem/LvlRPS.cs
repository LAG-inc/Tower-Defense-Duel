using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RpsLvl", menuName = "RpsCustom/RPSLvl", order = 0)]
public class LvlRps : ScriptableObject
{
    public int phasesInLvl;
    public float basePoints;
    public float waveFactor;
    public List<EnemyScriptable> enemiesAvailableStage1 = new List<EnemyScriptable>();
    public List<EnemyScriptable> enemiesAvailableStage2 = new List<EnemyScriptable>();
    public List<EnemyScriptable> enemiesAvailableStage3 = new List<EnemyScriptable>();
}