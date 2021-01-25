using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternConfigurator : MonoBehaviour
{
    [SerializeField, Tooltip("Pattern Points, no matter the order")]
    private List<EnemyPoint> patternPoints = new List<EnemyPoint>();

    private List<Collider2D> _enemyColliders = new List<Collider2D>();

    private void Awake()
    {
        patternPoints = patternPoints.OrderBy(enemyPoint => enemyPoint.ID).ToList();


        foreach (var point in patternPoints.Select(point => point.GetComponent<Collider2D>()))
        {
            _enemyColliders.Add(point);
        }
    }

    public void SetEnemyPoints(ref Enemy currentEnemy)
    {
        currentEnemy.SetEnemyPoints(patternPoints, _enemyColliders);
    }
}