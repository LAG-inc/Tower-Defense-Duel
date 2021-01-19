using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;

public class PoolManager : MonoBehaviour
{
    public static PoolManager SI;

    [Header("Allies")] [SerializeField] private BaseObjectPool _robot1Pool;
    [SerializeField] private BaseObjectPool _robot2Pool;

    [Header("Enemies")] [SerializeField] private BaseObjectPool _alien1Pool;

    [FormerlySerializedAs("objectsPools")] [FormerlySerializedAs("objPools")] [SerializeField]
    private List<BaseObjectPool> objectPools = new List<BaseObjectPool>();

    private void Awake()
    {
        if (SI == null)
        {
            SI = this;
        }

        /*
        _robot1Pool.FillQueue();
        _robot2Pool.FillQueue();
        */

        //_alien1Pool.FillQueue();
        foreach (var pool in objectPools)
        {
            pool.FillQueue(false);
        }
    }

    // Start is called before the first frame update


    /// <summary>
    /// Return a object pool by id, so, we can create as many object pools as we want
    /// </summary>
    /// <param name="poolId"></param>
    /// <returns></returns>
    public BaseObjectPool GetObjectPool(int poolId)
    {
        foreach (var pool in objectPools.Where(pool => pool.id == poolId))
        {
            return pool;
        }

        Debug.LogWarning($"{poolId} not found, returned an empty object pool");
        return ScriptableObject.CreateInstance<BaseObjectPool>();
    }


    public BaseObjectPool GetRobot1Pool()
    {
        return _robot1Pool;
    }

    public BaseObjectPool GetRobot2Pool()
    {
        return _robot2Pool;
    }
}