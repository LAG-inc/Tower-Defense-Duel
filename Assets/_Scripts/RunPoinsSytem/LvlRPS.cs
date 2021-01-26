using System.Collections.Generic;
using _Scripts.RunPoinsSytem;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "RpsLvl", menuName = "RpsCustom/RPSLvl", order = 0)]
public class LvlRps : ScriptableObject
{
    public int phasesInLvl;
    public int basePoints;
    public float waveFactor;
    public List<EnemiesStageList> enemiesAvailableStageI = new List<EnemiesStageList>();
}