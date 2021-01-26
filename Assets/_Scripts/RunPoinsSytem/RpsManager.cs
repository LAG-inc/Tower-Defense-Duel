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

    private float _currentPointsPhase;
    private int _currentPhase;

    private List<EnemyGenerator> _enemyGenerators = new List<EnemyGenerator>();

    public static RpsManager SingleInstance;


    private int[] _phasesPerStage = new int[3];

    private int _currentStage;

    private void Awake()
    {
        if (SingleInstance == null)
        {
            SingleInstance = this;
        }

        _currentStage = 1;
        _currentPhase = 1;
        _enemyGenerators.Clear();
    }


    private void Start()
    {
        _currentLvl = LvlHelper.SingleInstance.GetNextLvl();
        AssignLevelConfiguration();
        CalculatePhasesPerStage();
    }

    private void AssignLevelConfiguration()
    {
        _currentPointsPhase = _currentLvl.basePoints;
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
        if (_currentPhase > _currentLvl.phasesInLvl) return;


        var enemiesAvailable = new List<GenerableData>();

        
        if (_currentPhase == 1)
        {
            enemiesAvailable = _currentLvl.enemiesAvailableStage1;
        }
        else if(_currentPhase >= _phasesPerStage[_currentStage - 1])
        {
            _currentStage++;
            switch (_currentStage)
            {
                case 2:
                    enemiesAvailable = _currentLvl.enemiesAvailableStage2;
                    break;
                case 3:
                    enemiesAvailable = _currentLvl.enemiesAvailableStage3;
                    break;
            }
        }

        }

        _currentPointsPhase = CalculatePhasePoints(_currentPhase);

        var pointsPerGenerator = (int) Math.Ceiling(_currentPointsPhase / _enemyGenerators.Count);

        foreach (var enemyGenerator in _enemyGenerators)
        {
            enemyGenerator.SetAvailablePoints(pointsPerGenerator);
            enemyGenerator.SetEnemiesAvailable(enemiesAvailable);
        }

        _currentPhase++;
    }

    /// <summary>
    /// Calculate How many points will a new phase
    /// </summary>
    private int CalculatePhasePoints(int currentPhase)
    {
        //CurrentPoints + CurrentPhase * WaveFactor + fibonacci(phase/2)
        var form = _currentPointsPhase + currentPhase * _currentLvl.waveFactor +
                   UtilLag.Fibonacci(currentPhase / 2 + 1);
        return (int) form;
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
        var basePhaseNum = (int) Math.Ceiling((float) totalPhases / _phasesPerStage.Lenght);
        _phasesPerStage[0] = basePhaseNum;
        _phasesPerStage[1] = basePhaseNum * 2;
        _phasesPerStage[2] = _currentLvl.phasesInLvl;
    }


    public bool IsAnotherPhase()
    {
        return _currentPhase != _currentLvl.phasesInLvl;
    }
}
