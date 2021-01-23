using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : ThinkingGenerable
{
    private EType eType;
    private Rigidbody2D _rigidbody;
    [HideInInspector] public float speed;
    private int _currentTargetIndex;
    private Vector3 _target;
    //Set true when die animation finish (Machine State) ****
    public bool readyToDeactivate;

    private static List<Collider2D> _enemyPointsColliders = new List<Collider2D>();
    private static List<EnemyPoint> _enemyPointsScripts = new List<EnemyPoint>();

    private List<Vector3> _enemyPoints = new List<Vector3>();

    private static readonly int Attacking = Animator.StringToHash("Attacking");

    public enum EType
    {
        Alien1,
        Alien2,
        Alien3,
        None
    }

    private void Awake()
    {
        gType = GenerableType.Unit;

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody2D>();

    }

    private void Start()
    {
        _currentTargetIndex = 0;
        _target = _enemyPoints[_currentTargetIndex];
    }

    private void Update()
    {
        if (state == States.Attacking) return;

        if (Mathf.Abs(transform.position.x - _target.x) < 0.1f &&
            Mathf.Abs(transform.position.y - _target.y) < 0.1f)
        {
            ChangeTarget();
        }
    }

    private void FixedUpdate()
    {
        if (state == States.Attacking) return;

        _rigidbody.MovePosition(_rigidbody.position +
                                (Vector2)(_target - transform.position).normalized * (Time.deltaTime * speed));
    }

    public override void Activate(Faction gFaction, GenerableData gData)
    {
        eType = gData.eType;
        speed = gData.speed;
        base.Activate(gFaction, gData);
    }

    /// <summary>
    /// Change the current position to go named as target
    /// </summary>
    private void ChangeTarget()
    {
        if (_currentTargetIndex == _enemyPoints.Count - 1)
            StartAttack();
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

    //public void RecieveDamage(float damange)
    //{
    //    life -= damange;
    //    if (life <= 0)
    //    {
    //        StartCoroutine(DieBehavior());
    //    }
    //}

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

    //public void SetComponents(float lLife, Sprite sprite, float attackPower, Animator animator)
    //{
    //    life = lLife;
    //    _spriteRenderer.sprite = sprite;
    //    damage = attackPower;
    //    _animator = animator;
    //}

    private void SetAnimValues()
    {
        animator.SetBool(Attacking, false);
    }

    private void AssignEnemyPattern()
    {
        var currentEnemyPoints = new List<Vector3>();

        for (var index = 0; index < _enemyPointsColliders.Count; index++)
        {
            var point = _enemyPointsColliders[index];
            //TODO actualizar a los ultimos cambios que hizo Ritosan
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

    public override void SetTarget(ThinkingGenerable t)
    {
        base.SetTarget(t);
    }

    //Allied moves towards the target
    public override void Seek()
    {
        base.Seek();

        //animator.SetBool("IsMoving", true);
    }

    //Allied has gotten to its target. This function puts it in "attack mode", but doesn't delive any damage (see DealBlow)
    public override void StartAttack()
    {
        base.StartAttack();

        //animator.SetBool("IsMoving", false);
    }

    //Starts the attack animation, and is repeated according to the Allied's attackRatio
    public override void DealBlow()
    {
        base.DealBlow();

        //animator.SetTrigger("Attack");
        //TODO descomentar y acomodar
        //transform.forward = (target.transform.position - transform.position).normalized; //turn towards the target
    }

    public override void Stop()
    {
        base.Stop();

        //animator.SetBool("IsMoving", false);
    }

    protected override void Die()
    {
        base.Die();

        //animator.SetTrigger("IsDead");
    }

    public EType GetEType()
    {
        return eType;
    }
}