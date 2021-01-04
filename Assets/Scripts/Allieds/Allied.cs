using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Allied : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //PoolManager.SI.GetAlliesPool().ConfigPrefabEvent += (go) => Config(go);

    }

    private void Config(GameObject go)
    {
        Debug.Log("Entra a config desde Allied");
        Debug.Log(go.name);
    }
}
