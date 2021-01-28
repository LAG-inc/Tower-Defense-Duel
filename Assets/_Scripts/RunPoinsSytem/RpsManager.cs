using System;
using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent, DefaultExecutionOrder(-3000)]
public class RpsManager : MonoBehaviour
{
    /// <summary>
    /// Each Level Will have between 15-20 phases, these are gonna be divided in three stages. 
    /// </summary>
    private LvlRps _currentLvl;

    private int _currentPointsPhase;
    private int _currentPhase;

    private List<EnemyGenerator> _enemyGenerators = new List<EnemyGenerator>();

    public static RpsManager SingleInstance;


    private List<int> _phasesPerStage = new List<int>();

    private int _currentStage;


    public Action OnLastPhase;

    private void Awake()
    {
        _enemyGenerators.Clear();
        if (SingleInstance == null)
        {
            SingleInstance = this;
        }

        _currentStage = 1;
        _currentPhase = 0;
    }


    private void Start()
    {
        _currentLvl = LvlHelper.SingleInstance.GetNextLvl();
        // AssignLevelConfiguration();
        CalculatePhasesPerStage();
    }


    public void AddGenerator(EnemyGenerator newGenerator)
    {
        _enemyGenerators.Add(newGenerator);
    }

    public void SubtractGenerator(EnemyGenerator generatorToSubtract)
    {
        _enemyGenerators.Remove(generatorToSubtract);
    }


    /// <summary>
    /// It prepare the new phase for create enemies
    /// </summary>
    /// <returns></returns>
    public void PreparePhase()
    {
        _currentPhase++;

        if (_currentPhase >= _currentLvl.phasesInLvl)
            OnLastPhase.Invoke();

        var enemiesAvailable = new List<GenerableData>();

        if (_currentPhase == 1)
        {
            enemiesAvailable = _currentLvl.enemiesAvailableStageI[0].enemies;
        }
        else if (_currentPhase >= _phasesPerStage[_currentStage - 1])
        {
            for (var i = 1; i < _currentLvl.enemiesAvailableStageI.Count; i++)
            {
                if (_currentStage != i) continue;
                enemiesAvailable = _currentLvl.enemiesAvailableStageI[i].enemies;
                _currentStage++;
                break;
            }
        }

        _currentPointsPhase += CalculatePhasePoints(_currentPhase);

        var pointsPerGenerator = (int) Math.Ceiling((float) _currentPointsPhase / _enemyGenerators.Count);

        foreach (var enemyGenerator in _enemyGenerators)
        {
            enemyGenerator.SetAvailablePoints(pointsPerGenerator);
            enemyGenerator.SetEnemiesAvailable(enemiesAvailable);
        }
    }

    /// <summary>
    /// Calculate How many points will a new phase
    /// </summary>
    private int CalculatePhasePoints(int currentPhase)
    {
        //CurrentPoints + CurrentPhase * WaveFactor + fibonacci(phase/2)
        var form = _currentLvl.basePoints + currentPhase * _currentLvl.waveFactor +
                   UtilLag.Fibonacci(currentPhase / 2);

        return (int) Mathf.Ceil(form);
    }

    private void OnDisable()
    {
        _enemyGenerators.Clear();
    }

    /// <summary>
    /// Calculate how many phases its gonna have each stage.
    /// </summary>
    private void CalculatePhasesPerStage()
    {
        var totalPhases = _currentLvl.phasesInLvl;
        var basePhaseNum = (int) Math.Ceiling((float) totalPhases / _currentLvl.enemiesAvailableStageI.Count);

        for (var i = 0; i < _currentLvl.enemiesAvailableStageI.Count; i++)
        {
            if (i == _currentLvl.enemiesAvailableStageI.Count - 1)
                _phasesPerStage.Add(totalPhases);
            else
                _phasesPerStage.Add(basePhaseNum * (i + 1));
        }
    }
}