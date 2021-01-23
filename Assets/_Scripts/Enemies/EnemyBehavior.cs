using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBehavior : MonoBehaviour
{
    private float _speed;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;

    //Health system?
    [SerializeField, UnityEngine.Range(5, 30)]
    private float life;

    [SerializeField, UnityEngine.Range(5, 20)]
    private float damage;


    private int _currentTargetIndex;

    private Animator _animator;

    private Vector3 _target;

    //Set true when die animation finish (Machine State)
    public bool readyToDeactivate;


    private static List<Collider2D> _enemyPointsColliders = new List<Collider2D>();
    private static List<EnemyPoint> _enemyPointsScripts = new List<EnemyPoint>();


    private List<Vector3> _enemyPoints = new List<Vector3>();


    private static readonly int Attacking = Animator.StringToHash("Attacking");

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _currentTargetIndex = 0;
    }

    public void Update()
    {
        if (_enemyPoints.Count <= 0) return;

        if (Mathf.Abs(transform.position.x - _target.x) < 0.2f &&
            Mathf.Abs(transform.position.y - _target.y) < 0.2f)
        {
            ChangeTarget();
        }
    }

    private void FixedUpdate()
    {
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
        {
            Debug.Log("ATTACK THE BASE!!!!");
            //replace with Attack base and die
            Destroy(gameObject);
            return;
        }

        _currentTargetIndex++;

        _target = _enemyPoints[_currentTargetIndex];
    }

    /// <summary>
    /// Set the current points pattern of enemy
    /// </summary>
    /// <param name="enemyPointsScripts"></param>
    /// <param name="enemyPointsCollider"> Set the instance enemy pattern points</param>
    public void SetEnemyPoints(List<EnemyPoint> enemyPointsScripts, List<Collider2D> enemyPointsCollider)
    {
        _enemyPointsScripts = enemyPointsScripts;
        _enemyPointsColliders = enemyPointsCollider;
    }

    /// <summary>
    /// Change the speed of all the enemies
    /// </summary>
    /// <param name="damange"></param>
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
        StartCoroutine(AssignEnemyPattern());
        SetAnimValues();
    }


    /// <summary>
    /// Coroutine which allows disable the gameobject depending its current animation state
    /// </summary>
    /// <returns></returns>
    private IEnumerator DieBehavior()
    {
        yield return new WaitUntil(() => readyToDeactivate); // OnAnimationFinished in charge of make it true
        PoolManager.SI.GetObjectPool(1).EnqueueObj(gameObject);
        gameObject.SetActive(false);
    }


    public void SetComponents(float lLife, Sprite sprite, float attackPower, Animator animator, float speed)
    {
        _speed = speed;
        life = lLife;
        _spriteRenderer.sprite = sprite;
        damage = attackPower;
        _animator = animator;
    }


    private void SetAnimValues()
    {
        _animator.SetBool(Attacking, false);
    }


    private IEnumerator AssignEnemyPattern()
    {
        yield return new WaitUntil(() => _enemyPointsScripts.Count > 0);
        var currentEnemyPoints = new List<Vector3>();


        for (var index = 0; index < _enemyPointsColliders.Count; index++)
        {
            var point = _enemyPointsColliders[index];

            float randomY = Random.Range(point.bounds.min.y, point.bounds.max.y);


            float randomX = Random.Range(point.bounds.min.x, point.bounds.max.x);
            currentEnemyPoints.Add(new Vector3(randomX, randomY));
        }

        _enemyPoints = currentEnemyPoints;
    }
}