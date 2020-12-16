using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool : MonoBehaviour
{
    private Queue<GameObject> _objects = new Queue<GameObject>();

    //Prefab which contains all components of the object to instance
    [SerializeField] protected GameObject prefab;


    /// <summary>
    /// Instance a game object and make changes in itself
    /// </summary>
    /// <returns>The game object instanced</returns>
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