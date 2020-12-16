using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPool : ObjectPool
{
    [SerializeField, Tooltip("Pattern Points, no matter the order")]
    private List<Transform> enemyPoints = new List<Transform>();

    private void Awake()
    {
        enemyPoints = enemyPoints.OrderBy(enemyPoint => enemyPoint.gameObject.GetComponent<EnemyPoint>().ID).ToList();
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
        var obj = Instantiate(prefab, transform.position, transform.rotation, transform);

        var currentEnemy = obj.GetComponent<EnemyBehavior>();

        currentEnemy.SetEnemyPoints(enemyPoints);

        obj.SetActive(false);

        return obj;
    }
}