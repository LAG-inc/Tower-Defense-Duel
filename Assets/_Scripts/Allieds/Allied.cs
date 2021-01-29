using UnityEngine;

public class Allied : ThinkingGenerable
{
    private AType aType;
    [HideInInspector] public float creationTime;
    [HideInInspector] public int deployCost;
    [HideInInspector] public int energyPerAttack;

    public enum AType
    {
        Robot1,
        Robot2,
        Robot3,
        None
    }

    public Tile AttachedTile;

    private void Awake()
    {
        gType = GenerableType.Unit;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }


    public override void Activate(Faction gFaction, GenerableData gData)
    {
        aType = gData.aType; //Para identificar la pool
        creationTime = gData.creationTime;
        deployCost = gData.deployCost;
        energyPerAttack = gData.energyPerAttack;
        base.Activate(gFaction, gData);
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
        AttachedTile.SetIsEmpty(true);
        //animator.SetTrigger("IsDead");
    }

    public AType GetAType()
    {
        return aType;
    }
}
