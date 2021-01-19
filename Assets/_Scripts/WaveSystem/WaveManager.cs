﻿using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private int _currentLvl;
    [SerializeField] private int multiplier;

    [SerializeField, Header("Test")] private bool test;
    [SerializeField] private int enemiesPerTestPhase;
    [SerializeField] private float speedTest;

    private void Awake()
    {
        _currentLvl = PlayerPrefs.HasKey("Wave") ? PlayerPrefs.GetInt("Wave") : 1;

        ConfigureNewPhase();
    }

    /// <summary>
    /// Configure the new phase with a green duck form xD 
    /// </summary>
    private void ConfigureNewPhase()
    {
        if (!test)
        {
            var num = _currentLvl % 10 == 0
                ? UtilLag.Fibonacci(10) * multiplier + _currentLvl / 10 //  + spawnFactor * lvl
                : UtilLag.Fibonacci(_currentLvl % 10) * multiplier; //  + spawnFactor * lvl

            EnemyGenerator.ChangeEnemiesPhase(num);
            EnemyBehavior.ChangeSpeed(2);
        }
        else
        {
            EnemyGenerator.ChangeEnemiesPhase(enemiesPerTestPhase);
            EnemyBehavior.ChangeSpeed(speedTest);
        }
    }
    /*Idea:
     We can make a points system per lvl/enemy:
     lvl points: 200,  Minion: 10 points, Big enemy: 25 Points when we spawn an enemy, points are
     subtracted and in this way the lvl is easier to balance, i think.
     */

    /// <summary>
    /// Save the current phase and prepare the next phase TODO: Implement when the system will be finished
    /// </summary>
    public void WinPhase()
    {
        PlayerPrefs.SetInt("Wave", _currentLvl);
        _currentLvl++;
        ConfigureNewPhase();
    }


//Todo : At the end we will need gamestates, I think, cuz we need pause/game/prepare-phase 
}