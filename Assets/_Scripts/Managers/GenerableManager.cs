using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerableManager : MonoBehaviour
{
    public static GenerableManager Instance;

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
                            switch (p.faction)
                            {
                                case Generable.Faction.Player:
                                    if (p.attackType == ThinkingGenerable.AttackType.Ranged)
                                    {
                                        p.FireProjectile();
                                        p.bar.SetHealth(p.SufferDamage(((Allied) p).energyPerAttack));
                                    }

                                    break;
                                case Generable.Faction.Opponent:
                                    break;
                                case Generable.Faction.None:
                                    break;
                                default:
                                    break;
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
                //El objetivo podría estar muerto ya que este proyectil está volando
                if (currProjectile.target.state != ThinkingGenerable.States.Dead)
                {
                    float newHP = currProjectile.target.SufferDamage(currProjectile.damage);
                    currProjectile.target.bar.SetHealth(newHP);
                }
                currProjectile.gameObject.SetActive(false);
                PoolManager.SI.GetObjectPool("bullet1").EnqueueObj(currProjectile.gameObject);
                allProjectiles.RemoveAt(prjN);
            }
        }

    }

    public GameObject SetupGenerable(GenerableData gDataRef, Generable.Faction gFaction)
    {
        GameObject go = null;
        switch (gDataRef.gType)
        {
            case Generable.GenerableType.Unit:

                ThinkingGenerable uScript = null;

                switch (gFaction)
                {
                    case Generable.Faction.Player:

                        switch (gDataRef.aType)
                        {
                            case Allied.AType.Robot1:
                                go = PoolManager.SI.GetObjectPool("ally1").ExtractFromQueue();
                                break;
                            case Allied.AType.Robot2:
                                go = PoolManager.SI.GetObjectPool("ally2").ExtractFromQueue();
                                break;
                            default:
                                break;
                        }

                        uScript = go.GetComponent<Allied>();
                        break;
                    case Generable.Faction.Opponent:

                        //En este caso como tenemos una sola pool para todos los enemigos todos son type Alien1
                        //si esto cambia en un futuro hay que acomodarlo
                        switch (gDataRef.eType)
                        {
                            case Enemy.EType.Alien1:
                                go = PoolManager.SI.GetObjectPool("enemy1").ExtractFromQueue();
                                break;
                            default:
                                break;
                        }

                        uScript = go.GetComponent<Enemy>();
                        break;
                    case Generable.Faction.None:
                        break;
                    default:
                        break;
                }

                try
                {
                    uScript.Activate(gFaction, gDataRef);
                    uScript.OnDealDamage += OnGenerableDealtDamage;
                    uScript.OnProjectileFired += OnProjectileFired;
                    AddPlaceableToList(uScript);
                }
                catch (NullReferenceException e)
                {
                    throw new Exception("Falló el SetupGenerable", e);
                }

                break;
            default:
                break;
        }

        try
        {
            go.GetComponent<Generable>().OnDie += OnGenerableDead;
        }
        catch (NullReferenceException e)
        {
            throw new Exception("Fallo en el SetupGenerable, objeto nulo", e);
        }

        return go;
    }

    private void OnGenerableDealtDamage(ThinkingGenerable g)
    {
        if (g.target.state != ThinkingGenerable.States.Dead)
        {
            float newHealth = g.target.SufferDamage(g.damage);
            g.target.bar.SetHealth(newHealth);
        }
    }

    private void OnProjectileFired(ThinkingGenerable g)
    {
        Vector3 adjTargetPos = g.target.transform.position;
        Vector3 vectorToTarget = adjTargetPos - g.projectileSpawnPoint.position;

        Quaternion rot = Quaternion.LookRotation(Vector3.forward, vectorToTarget);

        GameObject prj = PoolManager.SI.GetObjectPool("bullet1").ExtractFromQueue();
        prj.transform.position = g.projectileSpawnPoint.position;
        prj.GetComponent<Projectile>().SetInitialPosition(g.projectileSpawnPoint.position);
        prj.transform.rotation = rot;

        prj.GetComponent<Projectile>().target = g.target;
        prj.GetComponent<Projectile>().damage = g.damage;

        prj.gameObject.SetActive(true);
        GetAllProjectiles().Add(prj.GetComponent<Projectile>());
    }

    private void OnGenerableDead(Generable g)
    {
        g.OnDie -= OnGenerableDead; //remove the listener

        switch (g.gType)
        {
            case Generable.GenerableType.Unit:

                ThinkingGenerable u = (ThinkingGenerable)g;

                RemoveGenerableFromList(u);

                u.OnDealDamage -= OnGenerableDealtDamage;
                u.OnProjectileFired -= OnProjectileFired;
                UIManager.SI.RemoveBar(u);

                switch (g.faction)
                {
                    case Generable.Faction.Player:

                        StartCoroutine(DisposeAllied((Allied)u));

                        break;
                    case Generable.Faction.Opponent:

                        StartCoroutine(DisposeEnemy((Enemy)u));

                        break;
                    case Generable.Faction.None:
                        break;
                    default:
                        break;
                }
                break;
        }
    }

    private IEnumerator DisposeAllied(Allied g)
    {
        //time for animation
        yield return new WaitForSeconds(0f);

        g.gameObject.SetActive(false);

        switch (g.GetAType())
        {
            case Allied.AType.Robot1:
                PoolManager.SI.GetObjectPool("ally1").EnqueueObj(g.gameObject);
                break;
            case Allied.AType.Robot2:
                PoolManager.SI.GetObjectPool("ally2").EnqueueObj(g.gameObject);
                break;
            case Allied.AType.Robot3:
                break;
            default:
                break;
        }

    }

    private IEnumerator DisposeEnemy(Enemy g)
    {
        //time for animation
        yield return new WaitForSeconds(0f);

        g.gameObject.SetActive(false);

        //En el caso de los enemigos se usa una sola pool, si en el futuro esto llega a cambiar
        //habria que acomodarlo
        PoolManager.SI.GetObjectPool("enemy1").EnqueueObj(g.gameObject);
    }

    public IEnumerator TimerEnergyAllied(float time, ThinkingGenerable obj)
    {
        for (float i = time * 10; i > 0; i--)
        {

            obj.bar.SetHealth(obj.SufferDamage(0.1f));

            yield return new WaitForSecondsRealtime(0.1f);
        }
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

    public void RemoveGenerableFromList(ThinkingGenerable g)
    {
        allThinkingPlaceables.Remove(g);

        if (g.faction == Generable.Faction.Player)
        {
            allPlayers.Remove(g);

            if (g.gType == Generable.GenerableType.Unit)
                playerUnits.Remove(g);

        }
        else if (g.faction == Generable.Faction.Opponent)
        {
            allOpponents.Remove(g);

            if (g.gType == Generable.GenerableType.Unit)
                opponentUnits.Remove(g);

        }
        else
        {
            Debug.LogError("Error in removing a Placeable from one of the player/opponent lists");
        }
    }
}
