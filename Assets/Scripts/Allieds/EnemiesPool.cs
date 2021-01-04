using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenericPool 1", menuName = "Custom/Pool/EnemiesPool")]
public class EnemiesPool : BaseObjectPool
{
    protected override GameObject CreateObj()
    {
        var obj = Instantiate(prefab, Vector3.up, Quaternion.Euler(0f, 180f, 0f), GameObject.Find("Pool").transform);

        obj.SetActive(false);

        return obj;
    }
}
