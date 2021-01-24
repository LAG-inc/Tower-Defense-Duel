using System.Collections.Generic;
using System.Runtime.InteropServices;
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

    private List<GenerableData> _enemies = new List<GenerableData>();

    private float _currentSpawnTime;

    private Collider2D _spawnZone;

    private static bool _canGenerate;

    private int _countDebug = 0;

    private int _availablePoints;

    private void Awake()
    {
        _canGenerate = false;

        _currentSpawnTime = 0;

        _enemyPool = PoolManager.SI.GetObjectPool(1);

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
        Debug.Log("YEAHHH I can generate Units!!!");

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

        var enemy = _enemyPool.ExtractFromQueue();


        GenerableData eData = _enemies[currentEnemy];

        Enemy eScript = enemy.GetComponent<Enemy>();
        eScript.Activate(eData.unitFaction, eData);


        //TODO estos dos metodos estan duplicados aqui y en Factory, se debe pasar este 
        //codigo a algun Manager, puede ser un futuro GenerableManager
        eScript.OnDealDamage += OnGenerableDealtDamage;
        eScript.OnProjectileFired += OnProjectileFired;

        GameManager.Instance.AddPlaceableToList(eScript);


        enemy.transform.SetParent(transform);

        var enemyBehavior = enemy.GetComponent<Enemy>();

        patternConfigurator.SetEnemyPoints(ref enemyBehavior);

        //enemyBehavior.SetComponents(_enemies[currentEnemy].life, _enemies[currentEnemy].enemySprite,
        //    _enemies[currentEnemy].damage
        //    , _enemies[currentEnemy].animator, _enemies[currentEnemy].speed);


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
        _enemies = enemyScriptable;
    }

    private void OnDisable()
    {
        RpsManager.SingleInstance.SubtractGenerator(this);
    }

    private void OnGenerableDealtDamage(ThinkingGenerable g)
    {
        if (g.target.state != ThinkingGenerable.States.Dead)
        {
            float newHealth = g.target.SufferDamage(g.damage);
            g.target.bar.SetHealth(newHealth);
        }
    }

    private void OnProjectileFired(ThinkingGenerable g)
    {
        Vector3 adjTargetPos = g.target.transform.position;
        Vector3 vectorToTarget = adjTargetPos - g.projectileSpawnPoint.position;

        Quaternion rot = Quaternion.LookRotation(Vector3.forward, vectorToTarget);

        GameObject prj = PoolManager.SI.GetBulletPool().ExtractFromQueue();
        prj.transform.position = g.projectileSpawnPoint.position;
        prj.GetComponent<Projectile>().SetInitialPosition(g.projectileSpawnPoint.position);
        prj.transform.rotation = rot;

        prj.GetComponent<Projectile>().target = g.target;
        prj.GetComponent<Projectile>().damage = g.damage;

        prj.gameObject.SetActive(true);
        GameManager.Instance.GetAllProjectiles().Add(prj.GetComponent<Projectile>());
    }
}