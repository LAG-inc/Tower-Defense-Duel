using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("Constant time between enemies in seconds"), Range(0, 5)]
    private float spawnRate;

    [SerializeField, Tooltip("Pattern Points, no matter the order")]
    private List<Transform> enemyPoints = new List<Transform>();

    [SerializeField, Tooltip("Enemies prefabs")]
    private List<GameObject> enemies = new List<GameObject>();


    [SerializeField] private int enemiesPerPhase;

    //A better name for _currentEnemiesPerPhase I think
    private int _enemiesSpawned;

    private float _currentSpawnTime;


    private void Start()
    {
        enemyPoints = enemyPoints.OrderBy(enemy => enemy.gameObject.GetComponent<EnemyPoint>().ID).ToList();
    }

    private void Update()
    {
        //return if pause

        if (_enemiesSpawned >= enemiesPerPhase)
        {
            return;
        }

        _currentSpawnTime += Time.deltaTime;
        if (!(_currentSpawnTime >= spawnRate)) return;
        _currentSpawnTime = 0;
        CreateEnemy();
    }

    private void CreateEnemy()
    {
        _enemiesSpawned++;
        var enemy = Instantiate(enemies[0], transform.position, transform.rotation);
        //Optimize this using queues and scriptables as in the last two games
        enemy.GetComponent<EnemyBehavior>().SetEnemyPoints(enemyPoints);
    }
}