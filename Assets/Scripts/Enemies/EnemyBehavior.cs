using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBehavior : MonoBehaviour
{
    private static float _speed;

    //Health system?
    [SerializeField, Range(5, 30)] private float life;
    [SerializeField, Range(5, 20)] private float damage;

    private List<Transform> _enemyPoints = new List<Transform>();
    private int _currentTargetIndex;
    private bool _attacking;

    private Vector3 _target;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _attacking = false;
        _rigidbody = GetComponent<Rigidbody2D>();
        GameManager.OnPause += () => _rigidbody.simulated = false;
    }

    private void Start()
    {
        _currentTargetIndex = 0;
        _target = _enemyPoints[_currentTargetIndex].position;
    }

    public void Update()
    {
        if (_attacking) return;
        if (Mathf.Abs(transform.position.x - _target.x) < 0.1f &&
            Mathf.Abs(transform.position.y - _target.y) < 0.1f)
        {
            ChangeTarget();
        }
    }

    private void FixedUpdate()
    {
        if (_attacking) return;
        _rigidbody.MovePosition(_rigidbody.position +
                                (Vector2) (_target - transform.position).normalized * (Time.deltaTime * _speed));
        Debug.Log(_rigidbody.velocity);
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lEnemyPoints"> Set the instance enemy pattern points</param>
    public void SetEnemyPoints(List<Transform> lEnemyPoints)
    {
        _enemyPoints = lEnemyPoints;
    }


    public static void ChangeSpeed(float speed)
    {
        _speed = speed;
    }
}