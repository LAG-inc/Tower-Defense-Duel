using UnityEngine;

public class FactoryTracker : MonoBehaviour
{
    [SerializeField, Tooltip("Cámara principal")]
    private Camera gameCamera;
    private static Factory _currentFactory;
    private static FactoryInteractive _currentInteractive;
    private static bool _canPlaceUnit;

    void Update()
    {
        if (_canPlaceUnit)
            PlaceUnit();
    }

    private void PlaceUnit()
    {
        if (Input.GetButtonDown("Action"))
        {
            Vector3 screenPos = Input.mousePosition;
            Vector3 worldPos = gameCamera.ScreenToWorldPoint(screenPos);
            _currentFactory.GenerateUnit(worldPos);
            _canPlaceUnit = false;
        }
    }

    public static void SetCurrentFactory(Factory factory, FactoryInteractive interactive)
    {
        if (_currentFactory)
            _currentInteractive.LoseFocus();
        _currentFactory = factory;
        _currentInteractive = interactive;
    }

    public static void SetCanPlaceUnit(bool canPlaceUnit=true)
    {
        _canPlaceUnit = canPlaceUnit;
    }
}
