using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<WavesScriptable> waveConfiguration = new List<WavesScriptable>();
    private int _currentPhase;


    private void Awake()
    {
        _currentPhase = PlayerPrefs.HasKey("Wave") ? PlayerPrefs.GetInt("Wave") : 0;
        ConfigureNewPhase();
    }

    /// <summary>
    /// Configure the new phase--- Pro:Easy Configurable --Cons: Need a scritapble per level(Largest game, more scriptables) 
    /// </summary>
    private void ConfigureNewPhase()
    {
        EnemyGenerator.ChangeEnemiesPhase(waveConfiguration[_currentPhase].enemiesInPhase);
        EnemyBehavior.ChangeSpeed(waveConfiguration[_currentPhase].enemyVelocity);
    }

    /// <summary>
    /// Save the current phase and prepare the next phase TODO: Implement when the system will be finished
    /// </summary>
    public void WinPhase()
    {
        PlayerPrefs.SetInt("Wave", _currentPhase);
        _currentPhase++;
        ConfigureNewPhase();
    }

    //Todo : At the end we will need gamestates, I think, cuz we need pause/game/prepare-phase 
}