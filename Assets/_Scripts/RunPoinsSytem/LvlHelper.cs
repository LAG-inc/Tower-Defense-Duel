using System;
using UnityEngine;

public class LvlHelper : MonoBehaviour
{
    private LvlRps _lvlSelected;
    public static LvlHelper SingleInstance;

    private void Awake()
    {
        if (SingleInstance == null)
        {
            SingleInstance = this;
        }
    }

    public LvlRps GetNextLvl()
    {
        return _lvlSelected;
    }

    public void SetNewLevel(LvlRps newLvl)
    {
        _lvlSelected = newLvl;
    }
}