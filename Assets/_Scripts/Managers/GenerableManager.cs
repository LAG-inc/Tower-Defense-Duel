using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerableManager : MonoBehaviour
{
    public static GenerableManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SetupGenerable(ref GameObject go, GenerableData gDataRef, Generable.Faction gFaction)
    {
        switch (gDataRef.gType)
        {
            case Generable.GenerableType.Unit:

                switch (gFaction)
                {
                    case Generable.Faction.Player:

                        switch (gDataRef.aType)
                        {
                            case Allied.AType.Robot1:
                                go = PoolManager.SI.GetRobot1Pool().ExtractFromQueue();
                                break;
                            case Allied.AType.Robot2:
                                go = PoolManager.SI.GetRobot2Pool().ExtractFromQueue();
                                break;
                            case Allied.AType.Robot3:
                                break;
                            default:
                                break;
                        }

                        break;
                    case Generable.Faction.Opponent:

                        switch (gDataRef.eType)
                        {
                            case Enemy.EType.Alien1:
                                break;
                            case Enemy.EType.Alien2:
                                break;
                            case Enemy.EType.Alien3:
                                break;
                            default:
                                break;
                        }

                        break;
                    case Generable.Faction.None:
                        break;
                    default:
                        break;
                }

                Allied uScript = go.GetComponent<Allied>();
                uScript.Activate(gFaction, gDataRef);
                uScript.OnDealDamage += OnGenerableDealtDamage;
                uScript.OnProjectileFired += OnProjectileFired;
                GameManager.Instance.AddPlaceableToList(uScript);
                //UIManager.AddHealthUI(uScript);

                break;
            case Generable.GenerableType.Resource:
                //TODO
                break;
            default:
                go = null;
                break;
        }

        go.GetComponent<Generable>().OnDie += OnGenerableDead;

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

        GameObject prj = PoolManager.SI.GetBulletPool().ExtractFromQueue();
        prj.transform.position = g.projectileSpawnPoint.position;
        prj.GetComponent<Projectile>().SetInitialPosition(g.projectileSpawnPoint.position);
        prj.transform.rotation = rot;

        prj.GetComponent<Projectile>().target = g.target;
        prj.GetComponent<Projectile>().damage = g.damage;

        prj.gameObject.SetActive(true);
        GameManager.Instance.GetAllProjectiles().Add(prj.GetComponent<Projectile>());
    }

    private void OnGenerableDead(Generable g)
    {
        g.OnDie -= OnGenerableDead; //remove the listener

        switch (g.gType)
        {
            case Generable.GenerableType.Unit:

                ThinkingGenerable u = (ThinkingGenerable)g;

                //TODO cambiar al pasar todo esto al GenerableManager
                GameManager.Instance.RemoveGenerableFromList(u);

                u.OnDealDamage -= OnGenerableDealtDamage;
                u.OnProjectileFired -= OnProjectileFired;
                UIManager.SI.RemoveBar(u);

                switch (g.faction)
                {
                    case Generable.Faction.Player:

                        //Esta corrutina solo existe en este script
                        StartCoroutine(DisposeAllied((Allied)u));

                        break;
                    case Generable.Faction.Opponent:

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
                PoolManager.SI.GetRobot1Pool().EnqueueObj(g.gameObject);
                break;
            case Allied.AType.Robot2:
                PoolManager.SI.GetRobot2Pool().EnqueueObj(g.gameObject);
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

        PoolManager.SI.GetObjectPool(1).EnqueueObj(gameObject);

        //switch (g.GetEType())
        //{
        //    case Enemy.EType.Alien1:
        //        //TODO
        //        break;
        //    case Enemy.EType.Alien2:
        //        //TODO
        //        break;
        //    case Enemy.EType.Alien3:
        //        break;
        //    case Enemy.EType.None:
        //        break;
        //    default:
        //        break;
        //}
    }

    public IEnumerator TimerEnergyAllied(float time, ThinkingGenerable obj)
    {
        for (float i = time * 10; i > 0; i--)
        {

            obj.bar.SetHealth(obj.SufferDamage(0.1f));

            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
