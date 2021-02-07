using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager SI;

    [Serializable]
    internal struct PoolItem
    {
        [Tooltip("Name of the object pool (to refer in scripts)")]
        [SerializeField] internal string PoolName;
        [Tooltip("Object pool Prefab")]
        [SerializeField] internal BaseObjectPool ObjectPool;
    };
    [SerializeField] private List<PoolItem> ObjectPools = new List<PoolItem>();

    private Dictionary<string, BaseObjectPool> _objectPools = new Dictionary<string, BaseObjectPool>();

    private void Awake()
    {
        if (SI == null)
        {
            SI = this;
        }
    }

    private void Start()
    {
        foreach (PoolItem pool in ObjectPools)
            _objectPools.Add(pool.PoolName.ToLower(), pool.ObjectPool);

        foreach (BaseObjectPool pool in _objectPools.Values)
        {
            try
            {
                pool.FillQueue();
            }
            catch (Exception e)
            {
                Debug.LogError("Object pool not found: " + e.ToString());
            }
        }
    }

    /// <summary>
    /// Return a object pool by name, so, we can create as many object pools as we want
    /// </summary>
    /// <param name="poolName">Name of the ObjectPool to get</param>
    /// <returns>BaseObjectPool</returns>
    public BaseObjectPool GetObjectPool(string poolName)
    {
        try
        {
            return _objectPools[poolName];
        }
        catch (Exception)
        {
            Debug.LogWarning($"{poolName} not found, returned an empty object pool instead.");
            return ScriptableObject.CreateInstance<BaseObjectPool>();
        }
    }
}
