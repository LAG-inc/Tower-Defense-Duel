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
    }

    private void Update()
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
        if (state == States.Attacking) return;

        _rigidbody.MovePosition(_rigidbody.position +
                                (Vector2)(_target - transform.position).normalized * (Time.deltaTime * speed));

        //Debug.Log(_enemyPointsScripts.Count, gameObject);

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
        if (_currentTargetIndex == _enemyPoints.Count)
        {
            Debug.Log("ATTACK THE BASE!!!!");
            //replace with Attack base and die
            //Destroy(gameObject);

            //Mato al enemigo
            this.bar.SetHealth(this.SufferDamage(this.hitPoints));
            _currentTargetIndex = 0;

            return;
        }
        _target = _enemyPoints[_currentTargetIndex];

        _currentTargetIndex++;

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
        StartCoroutine(AssignEnemyPattern());
        SetAnimValues();
    }

    ///// <summary>
    ///// Coroutine which allows disable the gameobject depending its current animation state
    ///// </summary>
    ///// <returns></returns>
    //private IEnumerator DieBehavior()
    //{
    //    yield return new WaitUntil(() => readyToDeactivate); // OnAnimationFinished in charge of make it true
    //    PoolManager.SI.GetObjectPool(1).EnqueueObj(gameObject);
    //    gameObject.SetActive(false);
    //}

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
        ChangeTarget();
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