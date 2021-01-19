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
    private List<EnemyPoint> enemyPoints = new List<EnemyPoint>();

    private List<Collider2D> _enemyPointsColliders = new List<Collider2D>();


    private void Awake()
    {
        _currentSpawnTime = 0;

        _enemyPool = PoolManager.SI.GetObjectPool(1);

        enemyPoints = enemyPoints.OrderBy(enemyPoint => enemyPoint.ID).ToList();

        foreach (var enemyCollider in enemyPoints.Select(enemyPoint => enemyPoint.GetComponent<Collider2D>()))
        {
            _enemyPointsColliders.Add(enemyCollider);
        }

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
        var currentEnemy = Random.Range(0, enemies.Count);

        var enemy = _enemyPool.ExtractFromQueue();

        enemy.transform.SetParent(transform);

        var enemyBehavior = enemy.GetComponent<EnemyBehavior>();
        enemyBehavior.SetComponents(enemies[currentEnemy].life, enemies[currentEnemy].enemySprite,
            enemies[currentEnemy].damage
            , enemies[currentEnemy].animator);

        var randomPos = Random.Range(_xPositionCollider.bounds.min.x, _xPositionCollider.bounds.max.x);
        enemy.transform.position = new Vector3(randomPos, transform.position.y);
        AssignEnemyValues(ref enemyBehavior);
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


    private void AssignEnemyValues(ref EnemyBehavior obj)
    {
        var currentEnemyPoints = new List<Vector3>();
        for (var index = 0; index < _enemyPointsColliders.Count; index++)
        {
            var point = _enemyPointsColliders[index];

            if (enemyPoints[index].direction == Direction.Vertical)
            {
                float randomY = Random.Range(point.bounds.min.y, point.bounds.max.y);
                currentEnemyPoints.Add(new Vector3(point.transform.position.x, randomY));
            }

            else
            {
                float randomX = Random.Range(point.bounds.min.x, point.bounds.max.x);
                currentEnemyPoints.Add(new Vector3(randomX, point.transform.position.y));
            }
        }

        obj.SetEnemyPointsColliders(currentEnemyPoints);

        obj.gameObject.SetActive(true);
    }
}