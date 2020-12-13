using UnityEngine;

public class Factory : MonoBehaviour
{
    private GameObject _unitToGenerate;

    public void PrepareUnitGeneration(GameObject unit)
    {
        Debug.Log($"Instanciar: {unit.name}");
        _unitToGenerate = unit;
        FactoryTracker.PlaceUnit();
    }

    public void GenerateUnit(Vector3 location)
    {
        var position = new Vector2(location.x, location.y);
        Instantiate(_unitToGenerate, position, Quaternion.identity);
    }
}
