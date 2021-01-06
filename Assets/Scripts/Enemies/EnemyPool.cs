using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPool : ObjectPool
{
    [SerializeField, Tooltip("Pattern Points, no matter the order")]
    private List<EnemyPoint> enemyPoints = new List<EnemyPoint>();

    private List<Collider2D> _enemyPointsColliders = new List<Collider2D>();

    private void Awake()
    {
        enemyPoints = enemyPoints.OrderBy(enemyPoint => enemyPoint.ID).ToList();


        foreach (var enemyCollider in enemyPoints.Select(enemyPoint => enemyPoint.GetComponent<Collider2D>()))
        {
            _enemyPointsColliders.Add(enemyCollider);
        }
    }


    private void Start()
    {
        FillQueue();
    }

    /// <summary>
    /// Create a new enemy and assigns its points to follow
    /// </summary>
    /// <returns></returns>
    protected override GameObject CreateObj()
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


        var obj = Instantiate(basePrefab, transform.position, transform.rotation, transform);

        var currentEnemy = obj.GetComponent<EnemyBehavior>();

        currentEnemy.SetEnemyPoints(currentEnemyPoints);

        obj.SetActive(false);

        return obj;
    }
}