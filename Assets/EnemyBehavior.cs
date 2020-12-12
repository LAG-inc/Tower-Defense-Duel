using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBehavior : MonoBehaviour
{
    [SerializeField, Range(1, 20)] private float speed;
    [SerializeField, Range(5, 30)] private float life;
    [SerializeField, Range(5, 20)] private float damage;
    private Rigidbody2D _rigidbody;

    private List<Transform> _enemyPoints = new List<Transform>();

    private Vector3 _target;
    private int _currentTargetIndex;
    private bool _attacking;

    private void Awake()
    {
        _attacking = false;
        foreach (var enemyPoint in FindObjectsOfType<EnemyPoint>())
        {
            _enemyPoints.Add(enemyPoint.transform);
        }

        _rigidbody = GetComponent<Rigidbody2D>();
        GameManager.OnPause += () => _rigidbody.simulated = false;
    }

    private void Start()
    {
        _currentTargetIndex = 0;
        _enemyPoints = _enemyPoints.OrderBy(enemies => enemies.gameObject.GetComponent<EnemyPoint>().ID).ToList();
        _target = _enemyPoints[_currentTargetIndex].position;
    }

    private void Update()
    {
        if (_attacking) return;

        if (Mathf.Abs(transform.position.x - _target.x) < 0.1f && Mathf.Abs(transform.position.y - _target.y) < 0.1f)
        {
            ChangeTarget();
        }
    }

    private void FixedUpdate()
    {
        if (_attacking) return;
        _rigidbody.MovePosition(_rigidbody.position +
                                (Vector2) (_target - transform.position).normalized * (Time.deltaTime * speed));
    }

    /// <summary>
    /// Change the current position to go named as target
    /// </summary>
    private void ChangeTarget()
    {
        if (_currentTargetIndex == _enemyPoints.Count - 1)
            _attacking = true;
        else
            _currentTargetIndex++;

        _target = _enemyPoints[_currentTargetIndex].position;
    }
}