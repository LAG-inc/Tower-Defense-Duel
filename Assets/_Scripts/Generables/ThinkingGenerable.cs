using UnityEngine;
using UnityEngine.Events;

public class ThinkingGenerable : Generable
{
    [HideInInspector] public States state = States.Generating;
    [HideInInspector] public AttackType attackType = AttackType.Ranged;
    [HideInInspector] public ThinkingGenerable target;
    [HideInInspector] public Bar bar;
    [HideInInspector] public float hitPoints; //Cuando las unidades sufren daño, ellos pierden hitPoints
    [HideInInspector] public float attackRange;
    [HideInInspector] public float attackRate; //Tiempo entre cada ataque
    [HideInInspector] public float lastBlowTime = -1000f;
    [HideInInspector] public float damage;
    [HideInInspector] public int cost;
    [HideInInspector] public float creationTime;
    [HideInInspector] public float deployTime;
    [HideInInspector] public AudioClip attackAudioClip;

    [HideInInspector] public float timeToActNext = 0f;

    [SerializeField] protected SpriteRenderer spriteRenderer;

    [Header("Projectile for Ranged")]
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    private Projectile projectile;
    protected AudioSource audioSource;
    protected Animator animator;

    public UnityAction<ThinkingGenerable> OnDealDamage, OnProjectileFired;

    public enum States
    {
        Generating,
        Idle,
        Seeking,
        Attacking,
        Dead
    }

    public enum AttackType
    {
        Melee,
        Ranged
    }

    public virtual void Activate(Faction gFaction, GenerableData gData)
    {
        GetComponent<SpriteRenderer>().sprite = gData.sprite;
        creationTime = gData.creationTime;
        deployTime = gData.deployTime;

        faction = gFaction;
        hitPoints = gData.hitPoints;
        targetType = gData.targetType;
        attackRange = gData.attackRange;
        attackRate = gData.attackRate;
        //speed = gData.speed;
        damage = gData.damagePerAttack;

        cost = gData.cost;
        dieAudioClip = gData.dieClip;

        attackAudioClip = gData.attackClip;

        state = States.Idle;

        UIManager.SI.AddBar(this);
    }

    public virtual void SetTarget(ThinkingGenerable t)
    {
        target = t;
        t.OnDie += TargetIsDead;
    }

    public virtual void StartAttack()
    {
        state = States.Attacking;
    }

    public virtual void DealBlow()
    {
        lastBlowTime = Time.time;
    }

    //Animation event hooks
    public void DealDamage()
    {
        if (attackType == AttackType.Melee)

            //Aqui se pueden reproducir los audios cuando hace daño

            if (OnDealDamage != null)
                OnDealDamage(this);
    }
    public void FireProjectile()
    {
        //Aqui se puede reproducir los audios de disparar proyectil

        if (OnProjectileFired != null)
            OnProjectileFired(this);
    }

    public virtual void Seek()
    {
        if (target == null)
            return;

        state = States.Seeking;
    }

    protected void TargetIsDead(Generable p)
    {
        state = States.Idle;

        target.OnDie -= TargetIsDead;

        timeToActNext = lastBlowTime + attackRate;
    }

    public bool IsTargetInRange()
    {
        return (transform.position - target.transform.position).sqrMagnitude <= attackRange * attackRange;
    }

    public float SufferDamage(float amount)
    {
        hitPoints -= amount;
        //Debug.Log("Suffering damage, new health: " + hitPoints, gameObject);

        if (state != States.Dead
            && hitPoints <= 0f)
        {
            Die();
        }

        return hitPoints;
    }

    public virtual void Stop()
    {
        state = States.Idle;
    }

    protected virtual void Die()
    {
        state = States.Dead;
        //Aqui se puede reproducir audio de muerte

        if (OnDie != null)
            OnDie(this);
    }
}
