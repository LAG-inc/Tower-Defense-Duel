using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField] private int maxNumberUnits;
    private int _currentNumberUnits;
    private bool _canGenerateUnit = true;
    private GameObject _unitToGenerate;

    public void PrepareUnitGeneration(GameObject unit)
    {
        _unitToGenerate = unit;
        FactoryTracker.SetCanPlaceUnit(true);
    }

    public void GenerateUnit(Vector2 location)
    {
        if (!_canGenerateUnit) return;
        Instantiate(_unitToGenerate, location, Quaternion.identity);
        _currentNumberUnits += 1;
        _canGenerateUnit = _currentNumberUnits == maxNumberUnits ? false : true;
    }

    public bool GetCanGenerateUnit() => _canGenerateUnit;
}
