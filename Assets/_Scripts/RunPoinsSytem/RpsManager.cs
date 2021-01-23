using System;
using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent, DefaultExecutionOrder(-3000)]
public class RpsManager : MonoBehaviour
{
    /// <summary>
    /// Each Level Will have between 15-20 phases, these are gonna be divided in three stages. 
    /// </summary>
    [SerializeField] private List<EnemyScriptable> enemiesAvalibleStage1 = new List<EnemyScriptable>();

    [SerializeField] private List<EnemyScriptable> enemiesAvalibleStage2 = new List<EnemyScriptable>();
    [SerializeField] private List<EnemyScriptable> enemiesAvalibleStage3 = new List<EnemyScriptable>();

    private LvlRps _currentLvl;
    private float _currentPointsPhase;
    private int _currentPhase;

    private static List<EnemyGenerator> _enemyGenerators = new List<EnemyGenerator>();

    public static RpsManager SingleInstance;


    private void Awake()
    {
        _currentPhase = 1;
        _enemyGenerators = null;
        if (SingleInstance == null)
        {
            SingleInstance = this;
        }
    }


    private void Start()
    {
        _currentLvl = LvlHelper.SingleInstance.GetNextLvl();
        AssignLevelConfiguration();
    }

    private void AssignLevelConfiguration()
    {
        _currentPointsPhase = _currentLvl.basePoints;
        enemiesAvalibleStage1 = _currentLvl.enemiesAvailableStage1;
        enemiesAvalibleStage2 = _currentLvl.enemiesAvailableStage2;
        enemiesAvalibleStage3 = _currentLvl.enemiesAvailableStage3;
    }

    public static void AddGenerator(EnemyGenerator newGenerator)
    {
        _enemyGenerators.Add(newGenerator);
    }


    /// <summary>
    /// It prepare the new phase for create enemies
    /// </summary>
    /// <returns></returns>
    private void PreparePhase()
    {
        CalculatePhasePoints();
        var pointsPerGenerator = (int) Math.Ceiling(_currentPointsPhase / _enemyGenerators.Count);

        foreach (var enemyGenerator in _enemyGenerators)
        {
            enemyGenerator.SetAvailablePoints(pointsPerGenerator);
        }
    }

    private void CalculatePhasePoints()
    {
        //CurrentPoints + CurrentPhase * WaveFactor + fibonacci(phase/2)
        var form = _currentPointsPhase + _currentPhase * _currentLvl.waveFactor +
                   UtilLag.Fibonacci(_currentPhase / 2 + 1);
        _currentPointsPhase = form;
    }

    private void OnDisable()
    {
        _enemyGenerators.Clear();
    }
}