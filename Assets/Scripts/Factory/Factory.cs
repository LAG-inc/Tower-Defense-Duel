using UnityEngine;

public class Factory : MonoBehaviour
{
    private GameObject _unitToGenerate;

    public void PrepareUnitGeneration(GameObject unit)
    {
        _unitToGenerate = unit;
        FactoryTracker.SetCanPlaceUnit(true);
    }

    public void GenerateUnit(Vector2 location)
    {
        if (_unitToGenerate)
            Instantiate(_unitToGenerate, location, Quaternion.identity);
    }
}
