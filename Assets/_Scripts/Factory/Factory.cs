﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField] private int maxNumberUnits;
    private int _currentNumberUnits;
    private bool _canGenerateUnit = true;
    private GenerableButton _buttonPressed;
    private GameObject _unitToGenerate;
    public bool CancelUnit { get; set; }

    //private float _creationTime;

    [SerializeField] private List<GenerableButton> _generableButtons;

    private void Start()
    {
        foreach (var button in _generableButtons)
        {
            button.OnMouseUp += PrepareUnitGeneration;
        }
    }

    public void PrepareUnitGeneration(GenerableButton button)
    {
        _buttonPressed = button;
        if (_buttonPressed.bar)
        {
            return;
        }
        CancelUnit = false;
        FactoryTracker.SetCanPlaceUnit(true);
    }

    public IEnumerator GenerateUnit(Vector2 location)
    {
        if (!_canGenerateUnit) yield break;

        GenerableButtonData bData = _buttonPressed.GetButtonData();

        //Timer para el tiempo de creacion
        yield return StartCoroutine(TimerSpawnAllied(bData.generablesData[0].creationTime, _buttonPressed));

        //Si cancela la creacion no se genera la unidad
        if (CancelUnit) yield break;

        DeployAlly(bData, location);
        
        _currentNumberUnits += bData.generablesData.Length;
        CheckGenerableButtonAvailability();
        _canGenerateUnit = _currentNumberUnits == maxNumberUnits ? false : true;

        //Timer para la 'energia' del aliado
        yield return StartCoroutine(GenerableManager.Instance.TimerEnergyAllied(bData.generablesData[0].hitPoints, _unitToGenerate.GetComponent<ThinkingGenerable>()));
    }

    public bool GetCanGenerateUnit() => _canGenerateUnit;

    private void CheckGenerableButtonAvailability()
    {
        foreach (var button in _generableButtons)
        {
            if((button.GetButtonData().generablesData.Length + _currentNumberUnits) > maxNumberUnits)
            {
                button.DisableButton();
            }
        }
    }

    private void DeployAlly(GenerableButtonData bData, Vector2 location)
    {
        for (int i = 0; i < bData.generablesData.Length; i++)
        {
            GenerableData gDataRef = bData.generablesData[i];
            GenerableManager.Instance.SetupGenerable(ref _unitToGenerate, gDataRef, gDataRef.unitFaction);
            _unitToGenerate.transform.position = location + bData.relativeOffsets[i];
            _unitToGenerate.SetActive(true);
        }
    }

    public IEnumerator TimerSpawnAllied(float time, GenerableButton obj)
    {
        UIManager.SI.AddBar(obj);

        for (float i = time*10; i > 0; i--)
        {
            if (CancelUnit) break;

            obj.bar.SetHealth((i/10) - 0.1f);

            yield return new WaitForSecondsRealtime(0.1f);
        }

        UIManager.SI.RemoveBar(obj);
    }

}
