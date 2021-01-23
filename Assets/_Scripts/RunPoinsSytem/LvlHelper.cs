using System;
using UnityEngine;

public class LvlHelper : MonoBehaviour
{
    [SerializeField] private LvlRps lvlSelected;
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
        return lvlSelected;
    }

    public void SetNewLevel(LvlRps newLvl)
    {
        lvlSelected = newLvl;
    }
}