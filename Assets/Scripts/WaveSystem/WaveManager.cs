using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private int _currentLvl;
    [SerializeField] private int multiplier;

    private void Awake()
    {
        _currentLvl = PlayerPrefs.HasKey("Wave") ? PlayerPrefs.GetInt("Wave") : 1;
        ConfigureNewPhase();
    }

    /// <summary>
    /// Configure the new phase--- Pro:Easy Configurable --Cons: Need a scritapble per level(Largest game, more scriptables) 
    /// </summary>
    private void ConfigureNewPhase()
    {
        var num = _currentLvl % 10 == 0
            ? UtilLag.Fibonacci(9) * multiplier + _currentLvl / 10 //  + spawnFactor * lvl
            : UtilLag.Fibonacci(_currentLvl % 10) * multiplier; //  + spawnFactor * lvl

        EnemyGenerator.ChangeEnemiesPhase(num);
        EnemyBehavior.ChangeSpeed(2);
    }

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