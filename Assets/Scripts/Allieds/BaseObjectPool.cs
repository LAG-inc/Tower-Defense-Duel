using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenericPool 1", menuName = "Custom/Pool/BasePool")]
public class BaseObjectPool : ScriptableObject
{
    protected Queue<GameObject> _objects = new Queue<GameObject>();

    //Prefab which contains all components of the object to instance
    [SerializeField] protected GameObject prefab;

    [SerializeField] protected int amount;

    /// <summary>
    /// Instance a game object and make changes in itself
    /// </summary>
    /// <returns>The game object instanced</returns>
    protected virtual GameObject CreateObj()
    {
        var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, GameObject.Find("Pool").transform);

        obj.SetActive(false);

        return obj;
    }

    /// <summary>
    /// Fill the  queue
    /// </summary>
    /// <param name="objQuantity"></param>
    public void FillQueue()
    {
        for (var i = 0; i < amount; i++)
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
        var obj = _objects.Dequeue();

        //obj.SetActive(true);

        return obj;
    }

    /// <summary>
    /// Return and obj to the que
    /// </summary>
    /// <param name="gameObject"></param>
    private void EnqueueObj(GameObject gameObject)
    {
        _objects.Enqueue(gameObject);
    }
}
