﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager SI;

    [SerializeField] private List<BaseObjectPool> ObjectPools;

    internal GameObject PoolGroup { get; private set; }
    private Dictionary<string, BaseObjectPool> _objectPoolsDic = new Dictionary<string, BaseObjectPool>();

    private void Awake()
    {
        if (SI == null)
        {
            SI = this;
        }
    }

    private void Start()
    {
        PoolGroup = GameObject.Find("Pool");

        foreach (BaseObjectPool pool in ObjectPools)
            _objectPoolsDic.Add(pool.poolName.ToLower(), pool);

        InitializeQueues();
    }

    private void InitializeQueues()
    {
        foreach (BaseObjectPool pool in _objectPoolsDic.Values)
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
            return _objectPoolsDic[poolName];
        }
        catch (Exception)
        {
            Debug.LogWarning($"Pool {poolName} not found, returned an empty object pool instead.");
            return ScriptableObject.CreateInstance<BaseObjectPool>();
        }
    }
}
