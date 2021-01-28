using System;
using UnityEngine;

public class PhaseInitializer : MonoBehaviour
{
    private void Awake()
    {
        RpsManager.SingleInstance.OnLastPhase += OnLastPhaseBehavior;
    }


    private void OnLastPhaseBehavior()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        RpsManager.SingleInstance.OnLastPhase -= OnLastPhaseBehavior;
    }


    private void OnMouseDown()
    {
        Debug.Log("Starting Phase");
        RpsManager.SingleInstance.PreparePhase();
        EnemyGenerator.StartGenerating();
    }
}