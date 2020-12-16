using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private EnemyPool enemyPool;

    [SerializeField, Tooltip("Constant time between enemies in seconds"), Range(0, 5)]
    private float spawnRate;

    [SerializeField, Tooltip("Enemies prefabs")]
    private List<EnemyScriptable> enemies = new List<EnemyScriptable>();


    private static int _enemiesPerPhase;

    private float _currentSpawnTime;


    private void Update()
    {
        //return if pause

        if (_enemiesPerPhase == 0) return;


        _currentSpawnTime += Time.deltaTime;

        if (!(_currentSpawnTime >= spawnRate)) return;
        _currentSpawnTime = 0;
        ActivateEnemy();
    }


    /// <summary>
    /// Activate enemy Prefab and assign it properties 
    /// </summary>
    private void ActivateEnemy()
    {
        enemyPool.ExtractFromQueue().GetComponent<EnemyBehavior>()
            .SetComponents(enemies[0].life, enemies[0].enemySprite, enemies[0].damage);
        _enemiesPerPhase--;
    }

    /// <summary>
    /// Change the quantity of enemies in phase
    /// </summary>
    /// <param name="numEnemies"></param>
    public static void ChangeEnemiesPhase(int numEnemies)
    {
        _enemiesPerPhase = numEnemies;
    }
}