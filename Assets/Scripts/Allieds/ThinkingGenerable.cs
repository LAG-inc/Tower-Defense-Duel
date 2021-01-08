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

    [Header("Projectile for Ranged")]
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    private Projectile projectile;
    protected AudioSource audioSource;

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
        //only melee units play audio when the attack deals damage
        if (attackType == AttackType.Melee)
            //audioSource.PlayOneShot(attackAudioClip, 1f);
            Debug.Log("Aqui deberia sonar cuando inflinge daño");

            if (OnDealDamage != null)
                OnDealDamage(this);
    }
    public void FireProjectile()
    {
        //ranged units play audio when the projectile is fired
        //audioSource.PlayOneShot(attackAudioClip, 1f);
        Debug.Log("Aqui deberia sonar cuando dispara un proyectil");

        if (OnProjectileFired != null)
            OnProjectileFired(this);
    }

    public virtual void Seek()
    {
        state = States.Seeking;
    }

    protected void TargetIsDead(Generable p)
    {
        //Debug.Log("My target " + p.name + " is dead", gameObject);
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
        //audioSource.pitch = Random.Range(.9f, 1.1f);
        //audioSource.PlayOneShot(dieAudioClip, 1f);

        if (OnDie != null)
            OnDie(this);
    }
}
