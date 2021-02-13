using UnityEngine;
using UnityEngine.Events;

public class ThinkingGenerable : Generable
{
    [HideInInspector] public States state = States.Generating;
    [HideInInspector] public AttackType attackType = AttackType.Ranged;
    [HideInInspector] public ThinkingGenerable target;
    [HideInInspector] public Bar bar;
    [HideInInspector] public float hitPoints; //Cuando las unidades sufren daño, ellos pierden hitPoints
    [HideInInspector] public float dyingPoints; 
    [HideInInspector] public float attackRange;
    [HideInInspector] public float attackRate; //Tiempo entre cada ataque
    [HideInInspector] public float lastBlowTime = -1000f;
    [HideInInspector] public float damage;
    [HideInInspector] public AudioClip attackAudioClip;

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
        Dying,
        Dead
    }

    public enum AttackType
    {
        Melee,
        Ranged
    }

    public virtual void Activate(Faction gFaction, GenerableData gData)
    {
        spriteRenderer.sprite = gData.sprite;

        faction = gFaction;
        hitPoints = gData.hitPoints;
        dyingPoints = gData.hitPoints;
        targetType = gData.targetType;
        attackRange = gData.attackRange;
        attackRate = gData.attackRate;
        //speed = gData.speed;
        damage = gData.damagePerAttack;

        dieAudioClip = gData.dieClip;

        attackAudioClip = gData.attackClip;

        state = States.Idle;

        UIManager.SI.AddBar(this);
    }

    public virtual void SetTarget(ThinkingGenerable t)
    {
        target = t;
        t.OnDying += TargetIsDying;
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

    protected void TargetIsDying(Generable g)
    {
        state = States.Idle;

        target.OnDying -= TargetIsDying;
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

    public void IsDying(float damage)
    {
        //Debug.Log("RESTA: " + (hitPoints - damage));

        dyingPoints -= damage;
        if(dyingPoints <= 0f)
        {
            Dying();
        }
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

    protected virtual void Dying()
    {
        state = States.Dying;
        if (OnDying != null)
            OnDying(this);
    }
}
