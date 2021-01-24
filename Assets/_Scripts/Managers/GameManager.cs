using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static event EventManager.VoidEvent OnPause;

    private List<ThinkingGenerable> playerUnits, opponentUnits;
    private List<ThinkingGenerable> allPlayers, allOpponents; 
    private List<ThinkingGenerable> allThinkingPlaceables;
    private List<Projectile> allProjectiles;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        playerUnits = new List<ThinkingGenerable>();
        opponentUnits = new List<ThinkingGenerable>();
        allPlayers = new List<ThinkingGenerable>();
        allOpponents = new List<ThinkingGenerable>();
        allThinkingPlaceables = new List<ThinkingGenerable>();
        allProjectiles = new List<Projectile>();
    }

    private void Update()
    {

        ThinkingGenerable targetToPass; 
        ThinkingGenerable p;

        for (int pN = 0; pN < allThinkingPlaceables.Count; pN++)
        {
            p = allThinkingPlaceables[pN];

            switch (p.state)
            {
                case ThinkingGenerable.States.Idle:
                    if (p.targetType == Generable.GenerableTarget.None)
                        break;

                    bool targetFound = FindClosestInList(p.transform.position, GetAttackList(p.faction, p.targetType), out targetToPass);
                    if (!targetFound)
                    {
                        Debug.LogWarning("No hay targets!");
                        continue;
                    }
                    p.SetTarget(targetToPass);
                    p.Seek();
                    break;


                case ThinkingGenerable.States.Seeking:
                    if (p.IsTargetInRange())
                    {
                        p.StartAttack();
                    }
                    break;


                case ThinkingGenerable.States.Attacking:
                    if (p.IsTargetInRange())
                    {
                        if (Time.time >= p.lastBlowTime + p.attackRate)
                        {
                            if (p.attackType == ThinkingGenerable.AttackType.Ranged)
                            {
                                p.FireProjectile();
                            }
                            p.DealBlow();
                        }
                    }
                    break;

            }
        }

        Projectile currProjectile;
        float progressToTarget;
        for (int prjN = 0; prjN < allProjectiles.Count; prjN++)
        { 
            currProjectile = allProjectiles[prjN];
            progressToTarget = currProjectile.Move();
            if (progressToTarget >= 1f)
            {
                if (currProjectile.target.state != ThinkingGenerable.States.Dead) //el objetivo podría estar muerto ya que este proyectil está volando
                {
                    float newHP = currProjectile.target.SufferDamage(currProjectile.damage);
                    //TODO descomentar
                    //currProjectile.target.bar.SetHealth(newHP);
                }
                currProjectile.gameObject.SetActive(false);
                PoolManager.SI.GetBulletPool().EnqueueObj(currProjectile.gameObject);
                allProjectiles.RemoveAt(prjN);
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
    }

    /// <summary>
    /// Invoke Game Pause.
    /// To add a behavior to the event---- GameManager.OnPause+= [void func] / () => [single behavior]  
    /// </summary>
    public static void Pause()
    {
        OnPause?.Invoke();
    }

    private void OnDestroy()
    {
        OnPause = null;
    }

    public void AddPlaceableToList(ThinkingGenerable p)
    {
        allThinkingPlaceables.Add(p);

        if (p.faction == Generable.Faction.Player)
        {
            allPlayers.Add(p);

            if (p.gType == Generable.GenerableType.Unit)
            {
                playerUnits.Add(p);
            }
            else
            {
                //usar cuando tengamos mas tipos de generables
            }
        }
        else if (p.faction == Generable.Faction.Opponent)
        {
            allOpponents.Add(p);

            if (p.gType == Generable.GenerableType.Unit)
            {
                opponentUnits.Add(p);
            }
            else
            {
                //usar cuando tengamos mas tipos de generables
            }
        }
        else
        {
            Debug.LogError("Error al agregar un Generable a una lista de oponentes/aliados");
        }
    }

    private bool FindClosestInList(Vector3 p, List<ThinkingGenerable> list, out ThinkingGenerable t)
    {
        t = null;
        bool targetFound = false;
        float closestDistanceSqr = Mathf.Infinity; 

        for (int i = 0; i < list.Count; i++)
        {
            float sqrDistance = (p - list[i].transform.position).sqrMagnitude;

            if (sqrDistance < closestDistanceSqr)
            {
                t = list[i];
                closestDistanceSqr = sqrDistance;
                targetFound = true;
            }
        }

        return targetFound;
    }

    private List<ThinkingGenerable> GetAttackList(Generable.Faction f, Generable.GenerableTarget t)
    {
        switch (t)
        {
            case Generable.GenerableTarget.Unit:
                return (f == Generable.Faction.Player) ? allOpponents : allPlayers;
            case Generable.GenerableTarget.OnlyBuildings:
            default:
                Debug.LogError("Que faccion es??");
                return null;
        }
    }

    public List<Projectile> GetAllProjectiles()
    {
        return allProjectiles;
    }
}