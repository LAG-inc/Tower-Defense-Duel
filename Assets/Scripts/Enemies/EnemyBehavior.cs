using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBehavior : MonoBehaviour
{
    private static float _speed;
    private SpriteRenderer _spriteRenderer;

    //Health system?
    [SerializeField, Range(5, 30)] private float life;
    [SerializeField, Range(5, 20)] private float damage;

    private List<Transform> _enemyPoints = new List<Transform>();
    private int _currentTargetIndex;
    private bool _attacking;

    private Vector3 _target;
    private Rigidbody2D _rigidbody;

    //Set true when die animation finish (Machine State)
    public bool readyToDeactivate;

    private void Awake()
    {
        _attacking = false;
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
        _target = _enemyPoints[_currentTargetIndex].position;
    }

    /// <summary>
    /// Set the current points pattern of enemy
    /// </summary>
    /// <param name="lEnemyPoints"> Set the instance enemy pattern points</param>
    public void SetEnemyPoints(List<Transform> lEnemyPoints)
    {
        _enemyPoints = lEnemyPoints;
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


    private void OnDisable()
    {
        //Todo: return to currentObject pool
    }

    /// <summary>
    /// Coroutine which allows disable the gameobject depending its current animation state
    /// </summary>
    /// <returns></returns>
    private IEnumerator DieBehavior()
    {
        yield return new WaitUntil(() => readyToDeactivate);
        gameObject.SetActive(false);
    }


    public void SetComponents(float lLife, Sprite sprite, float attackPower)
    {
        life = lLife;
        _spriteRenderer.sprite = sprite;
        damage = attackPower;
    }
}