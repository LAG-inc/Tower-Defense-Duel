using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenericPool 1", menuName = "Custom/Pool/BasePool")]
public class BaseObjectPool : ScriptableObject
{
    protected Queue<GameObject> _objects = new Queue<GameObject>();

    public int id;

    //Prefab which contains all components of the object to instance
    [SerializeField] protected GameObject prefab;

    [SerializeField] protected int amount;

    /// <summary>
    /// Instance a game object and make changes in itself
    /// </summary>
    /// <returns>The game object instanced</returns>
    protected virtual GameObject CreateObj(bool customIns = true)
    {
        if (customIns)
        {
            var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, GameObject.Find("Pool").transform);
            obj.SetActive(false);
            return obj;
        }
        else
        {
            var obj = Instantiate(prefab);
            obj.SetActive(false);
            return obj;
        }
    }

    /// <summary>
    /// Fill the  queue
    /// </summary>
    public void FillQueue(bool custom = true)
    {
        for (var i = 0; i < amount; i++)
        {
            EnqueueObj(CreateObj(custom));
        }
    }

    /// <summary>
    /// Return an object of the pool, if the pool is almost empty is filled again
    /// </summary>
    /// <returns></returns>
    public GameObject ExtractFromQueue()
    {
        if (_objects.Count < 2)
        {
            FillQueue(false);
        }

        var obj = _objects.Dequeue();

        obj.SetActive(true);

        return obj;
    }

    /// <summary>
    /// Return and obj to the que
    /// </summary>
    /// <param name="gameObject"></param>
    public void EnqueueObj(GameObject gameObject)
    {
        _objects.Enqueue(gameObject);
    }
}