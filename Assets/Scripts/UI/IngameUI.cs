using UnityEngine;

public class IngameUI : MonoBehaviour
{
    public void AddFactory(GameObject factory)
    {
        FactoryTracker.PrepareFactoryGeneration(factory);
    }
}
