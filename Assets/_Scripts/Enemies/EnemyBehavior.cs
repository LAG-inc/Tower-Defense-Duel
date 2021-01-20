using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBehavior : MonoBehaviour
{
    private static float _speed;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;

    //Health system?
    [SerializeField, UnityEngine.Range(5, 30)]
    private float life;

    [SerializeField, UnityEngine.Range(5, 20)]
    private float damage;

    [SerializeField, UnityEngine.Range(5, 20)]
    private float attackeSpeed;

    private int _currentTargetIndex;

    private Animator _animator;

    private Vector3 _target;

    //Set true when die animation finish (Machine State)
    public bool readyToDeactivate;

    private bool _attacking;

    private static List<Collider2D> _enemyPointsColliders = new List<Collider2D>();
    private static List<EnemyPoint> _enemyPointsScripts = new List<EnemyPoint>();


    private List<Vector3> _enemyPoints = new List<Vector3>();


    private static readonly int Attacking = Animator.StringToHash("Attacking");

    private void Awake()
    {
        _attacking = false;
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _currentTargetIndex = 0;
        _target = _enemyPoints[_currentTargetIndex];
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

        // if (_attacking) return;
        _rigidbody.MovePosition(_rigidbody.position +
                                (Vector2) (_target - transform.position).normalized * (Time.deltaTime * _speed));
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

        _target = _enemyPoints[_currentTargetIndex];
    }

    /// <summary>
    /// Set the current points pattern of enemy
    /// </summary>
    /// <param name="enemyPointsScripts"></param>
    /// <param name="enemyPointsCollider"> Set the instance enemy pattern points</param>
    public static void SetEnemyPoints(List<EnemyPoint> enemyPointsScripts, List<Collider2D> enemyPointsCollider)
    {
        _enemyPointsScripts = enemyPointsScripts;
        _enemyPointsColliders = enemyPointsCollider;
    }

    /// <summary>
    /// Change the speed of all the enemies
    /// </summary>
    /// <param name="speed"></param>
    public static void ChangeSpeed(float speed)
    {
        _speed = speed;
    }


    public void RecieveDamage(float damange)
    {
        life -= damange;
        if (life <= 0)
        {
            StartCoroutine(DieBehavior());
        }
    }


    private void OnEnable()
    {
        AssignEnemyPattern();
        SetAnimValues();
    }


    /// <summary>
    /// Coroutine which allows disable the gameobject depending its current animation state
    /// </summary>
    /// <returns></returns>
    private IEnumerator DieBehavior()
    {
        yield return new WaitUntil(() => readyToDeactivate); // OnAnimationFinished
        PoolManager.SI.GetObjectPool(1).EnqueueObj(gameObject);
        gameObject.SetActive(false);
    }


    public void SetComponents(float lLife, Sprite sprite, float attackPower, Animator animator)
    {
        life = lLife;
        _spriteRenderer.sprite = sprite;
        damage = attackPower;
        _animator = animator;
    }


    private void SetAnimValues()
    {
        _animator.SetBool(Attacking, false);
    }


    private void AssignEnemyPattern()
    {
        var currentEnemyPoints = new List<Vector3>();


        for (var index = 0; index < _enemyPointsColliders.Count; index++)
        {
            var point = _enemyPointsColliders[index];

            if (_enemyPointsScripts[index].direction == Direction.Vertical)
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

        _enemyPoints = currentEnemyPoints;
    }
}