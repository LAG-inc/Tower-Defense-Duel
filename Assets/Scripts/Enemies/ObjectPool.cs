using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool : MonoBehaviour
{
    protected Queue<GameObject> _objects = new Queue<GameObject>();
    [SerializeField] protected GameObject prefab;


    /// <summary>
    /// Instance a ned game object and make changes in itself
    /// </summary>
    /// <returns></returns>
    protected abstract GameObject CreateObj();

    /// <summary>
    /// Fill the  queue
    /// </summary>
    /// <param name="objQuantity"></param>
    protected void FillQueue(int objQuantity = 10)
    {
        for (var i = 0; i < objQuantity; i++)
        {
            EnqueueObj(CreateObj());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public GameObject ExtractFromQueue()
    {
        if (_objects.Count < 2)
        {
            FillQueue(5);
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