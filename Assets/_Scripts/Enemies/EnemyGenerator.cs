using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[DefaultExecutionOrder(3000)]
public class EnemyGenerator : MonoBehaviour
{
    private BaseObjectPool _enemyPool;

    [SerializeField, Tooltip("Constant time between enemies in seconds"), Range(0, 5)]
    private float spawnRate;

    [SerializeField, Tooltip("Enemies prefabs")]
    private List<EnemyScriptable> enemies = new List<EnemyScriptable>();


    private static int _enemiesPerPhase;
    private float _currentSpawnTime;

    private Collider2D _xPositionCollider;

    [SerializeField, Tooltip("Pattern Points, no matter the order")]
    private List<EnemyPoint> patternPoints = new List<EnemyPoint>();


    private int _countDebug = 0;

    private void Awake()
    {
        patternPoints = patternPoints.OrderBy(enemyPoint => enemyPoint.ID).ToList();

        var enemyColliders = patternPoints.Select(enemyPoint => enemyPoint.GetComponent<Collider2D>())
            .ToList();

        EnemyBehavior.SetEnemyPoints(patternPoints, enemyColliders);


        _currentSpawnTime = 0;

        _enemyPool = PoolManager.SI.GetObjectPool(1);


        _xPositionCollider = GetComponent<Collider2D>();
    }


    private void Update()
    {
        if (_enemiesPerPhase <= 0) return;

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
        _countDebug++;
        var currentEnemy = Random.Range(0, enemies.Count);

        var enemy = _enemyPool.ExtractFromQueue();


        enemy.transform.SetParent(transform);

        var enemyBehavior = enemy.GetComponent<EnemyBehavior>();

        enemyBehavior.SetComponents(enemies[currentEnemy].life, enemies[currentEnemy].enemySprite,
            enemies[currentEnemy].damage
            , enemies[currentEnemy].animator);

        var randomPos = Random.Range(_xPositionCollider.bounds.min.x, _xPositionCollider.bounds.max.x);

        enemy.transform.position = new Vector3(randomPos, transform.position.y);
        _enemiesPerPhase--;

        enemy.gameObject.SetActive(true);
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