using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PatternConfigurator))]
[DefaultExecutionOrder(3000), DisallowMultipleComponent, RequireComponent(typeof(Collider2D))]
public class EnemyGenerator : MonoBehaviour
{
    public PatternConfigurator patternConfigurator;
    private BaseObjectPool _enemyPool;

    [SerializeField, Tooltip("Constant time between enemies in seconds"), Range(0, 5)]
    private float spawnRate;

    [SerializeField, Tooltip("Enemies prefabs")]
    private List<EnemyScriptable> enemies = new List<EnemyScriptable>();

    private float _currentSpawnTime;

    private Collider2D _xPositionCollider;

    private static bool _canGenerate;

    private int _countDebug = 0;

    private int _availablePoints;

    private void Awake()
    {
        _canGenerate = false;

        RpsManager.AddGenerator(this);


        _currentSpawnTime = 0;

        _enemyPool = PoolManager.SI.GetObjectPool(1);

        _xPositionCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        RpsManager.AddGenerator(this);
    }

    private void Update()
    {
        if (!_canGenerate)
            return;


        if (_availablePoints <= 0)
        {
            _canGenerate = false;
            return;
        }

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

        patternConfigurator.SetEnemyPoints(ref enemyBehavior);

        enemyBehavior.SetComponents(enemies[currentEnemy].life, enemies[currentEnemy].enemySprite,
            enemies[currentEnemy].damage
            , enemies[currentEnemy].animator, enemies[currentEnemy].speed);


        var randomPos = Random.Range(_xPositionCollider.bounds.min.x, _xPositionCollider.bounds.max.x);

        enemy.transform.position = new Vector3(randomPos, transform.position.y);

        _availablePoints -= enemies[currentEnemy].points;
    }


    public void SetAvailablePoints(int points)
    {
        _availablePoints = points;
    }

    public static void StartGenerating()
    {
        _canGenerate = true;
    }
}