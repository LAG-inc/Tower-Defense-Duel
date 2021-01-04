using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //PoolManager.SI.GetEnemiesPool().ConfigPrefabEvent += (go) => Config(go);

    }

    private void Config(GameObject go)
    {
        Debug.Log("Entra a config desde Enemy");
        Debug.Log(go.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
