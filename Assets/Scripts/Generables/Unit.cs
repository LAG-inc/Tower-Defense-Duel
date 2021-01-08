using UnityEngine;

public class Unit : ThinkingGenerable
{
    //TODO: Vamos a utilizar los eventos que ellos hicieron, concretamente el EventManager (Dani
    // ya lo uso por lo tanto puedo guiarme por alli)

    //TODO el primer paso es crear la pool personalizable de forma que de alli pueda 
    // sacar todos mis aliados y enemigos ##COMPLETADO##

    //TODO: luego debo conectar mi tipo de unidad 
    // con la que el jugador clico en la fabrica, luego en el metodo Factory.GenerateUnit 
    // debo activar mi unidad en la posicion que ese metodo me provee y luego ##COMPLETADO##

    // ya empieza el cooldown y la parte de como va a atacar el aliado

    //Data coming from the PlaceableData

    //TODO: Revisar si la List de ActiveTiles realmente esta guardando solo tiles activos, revisar
    //Con el debugger de VS

    //TODO: Plantear cambiar el script monoBeheavior de Factory a un ScriptableObject de forma que
    //se puedan crear distintos tipos de fabricas

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

        animator.SetTrigger("IsDead");
    }
}
