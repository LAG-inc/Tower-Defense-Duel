using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.Examples;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PatternConfigurator))]
[DefaultExecutionOrder(3000), DisallowMultipleComponent, RequireComponent(typeof(Collider2D))]
public class EnemyGenerator : MonoBehaviour
{
    public PatternConfigurator patternConfigurator;

    [SerializeField, Tooltip("Constant time between enemies in seconds"), Range(0, 5)]
    private float spawnRate;

    private List<GenerableData> _enemies = new List<GenerableData>();

    private float _currentSpawnTime;

    private Collider2D _spawnZone;

    private static bool _canGenerate;

    private int _countDebug = 0;

    private int _availablePoints;

    private void Awake()
    {
        _canGenerate = false;
        _availablePoints = 0;
        _currentSpawnTime = 0;
        _spawnZone = GetComponent<Collider2D>();
    }

    private void Start()
    {
        var a = this;
        RpsManager.SingleInstance.AddGenerator(this);
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

        var currentEnemy = Random.Range(0, _enemies.Count);

        //var enemy = _enemyPool.ExtractFromQueue();
        GameObject enemy = new GameObject();
        //enemy.SetActive(false);

        GenerableData eData = _enemies[currentEnemy];

        GenerableManager.Instance.SetupGenerable(ref enemy, eData, eData.unitFaction);

        enemy.transform.SetParent(transform);

        //Refactor
        //Esto no lo puedo hacer en GenerableManager porque cada EnemyGenerator necesita un PatternConfigurator
        var enemyBehavior = enemy.GetComponent<Enemy>();
        patternConfigurator.SetEnemyPoints(ref enemyBehavior);


        var randomPos = Random.Range(_spawnZone.bounds.min.x, _spawnZone.bounds.max.x);

        enemy.transform.position = new Vector3(randomPos, transform.position.y);

        _availablePoints -= _enemies[currentEnemy].points;
    }

    public void SetAvailablePoints(int points)
    {
        _availablePoints = points;
    }

    public static void StartGenerating()
    {
        _canGenerate = true;
    }

    public void SetEnemiesAvailable(List<GenerableData> enemyScriptable)
    {
        foreach (var enemy in enemyScriptable.Where(enemy => !_enemies.Contains(enemy)))
        {
            _enemies.Add(enemy);
        }
    }

    private void OnDisable()
    {
        RpsManager.SingleInstance.SubtractGenerator(this);
    }
}