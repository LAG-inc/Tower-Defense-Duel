using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenericPool 1", menuName = "Custom/Pool/BasePool")]
public class BaseObjectPool : ScriptableObject
{
    [SerializeField] internal string poolName;
    //Prefab which contains all components of the object to instance
    [SerializeField] protected GameObject prefab;
    [SerializeField] protected int amount;

    protected Queue<GameObject> _objects = new Queue<GameObject>();

    /// <summary>
    /// Instance a game object and make changes in itself
    /// </summary>
    /// <returns>The game object instanced</returns>
    protected virtual GameObject CreateObj()
    {
        var obj = Instantiate(
            prefab,
            Vector3.zero,
            Quaternion.identity,
            PoolManager.SI.PoolGroup?.transform
        );
        obj.SetActive(false);
        return obj;
    }

    /// <summary>
    /// Fill the queue
    /// </summary>
    public void FillQueue()
    {
        for (var i = 0; i < amount; i++)
        {
            EnqueueObj(CreateObj());
        }
    }

    /// <summary>
    /// Extract an GameObject from the pool, if the pool is almost empty is filled again
    /// </summary>
    /// <returns>GameObject</returns>
    public GameObject ExtractFromQueue()
    {
        if (_objects.Count < 4)
        {
            FillQueue();
        }

        var obj = _objects.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    /// <summary>
    /// Adds and GameObject to the queue
    /// </summary>
    /// <param name="gameObject"></param>
    public void EnqueueObj(GameObject gameObject) => _objects.Enqueue(gameObject);
}
