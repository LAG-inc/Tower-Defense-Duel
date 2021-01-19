using UnityEngine;

public class Unit : ThinkingGenerable
{
    

    //private float speed;

    private Animator animator;

    private UnitType uType;

    private void Awake()
    {
        gType = GenerableType.Unit;

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public enum UnitType
    {
        Robot1,
        Robot2,
        Robot3
    }

    public void Activate(GenerableData gData)
    {
        uType = gData.unityType; //Para identificar la pool
        GetComponent<SpriteRenderer>().sprite = gData.sprite;
        creationTime = gData.creationTime;
        deployTime = gData.deployTime;

        //faction = gFaction;
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

    public override void SetTarget(ThinkingGenerable t)
    {
        base.SetTarget(t);
    }

    //Unit moves towards the target
    public override void Seek()
    {
        if (target == null)
            return;

        base.Seek();

        animator.SetBool("IsMoving", true);
    }

    //Unit has gotten to its target. This function puts it in "attack mode", but doesn't delive any damage (see DealBlow)
    public override void StartAttack()
    {
        base.StartAttack();

        animator.SetBool("IsMoving", false);
    }

    //Starts the attack animation, and is repeated according to the Unit's attackRatio
    public override void DealBlow()
    {
        base.DealBlow();

        animator.SetTrigger("Attack");
        transform.forward = (target.transform.position - transform.position).normalized; //turn towards the target
    }

    public override void Stop()
    {
        base.Stop();

        animator.SetBool("IsMoving", false);
    }

    protected override void Die()
    {
        base.Die();

        //animator.SetTrigger("IsDead");
    }

    public UnitType GetUnitType()
    {
        return uType;
    }
}
