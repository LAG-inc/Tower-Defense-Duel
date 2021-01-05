using UnityEngine;

public class IngameUI : MonoBehaviour
{
    public void AddFactory(GameObject factory)
    {
        FactoryTracker.SetFactoryToGenerate(factory);
        FactoryTracker.SetCanPlaceFactory(true);
    }
}
