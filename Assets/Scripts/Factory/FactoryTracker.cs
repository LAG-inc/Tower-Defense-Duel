using UnityEngine;

public class FactoryTracker : MonoBehaviour
{
    private static Factory _currentFactory;
    private static FactoryInteractive _currentInteractive;
    private static bool _canPlaceUnit;

    void Start()
    {
        Tile.OnMouseClick += () => PlaceUnit();
    }

    private void PlaceUnit()
    {
        if (!_canPlaceUnit) return;
        RaycastHit2D hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Input.mousePosition),
            Vector2.zero,
            10.0f,
            LayerMask.GetMask("Tile"));
        if (hit)
        {
            // TODO: pass game object to have the reference
            _currentFactory?.GenerateUnit(hit.collider.transform.position);
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

    public static void SetCanPlaceUnit(bool canPlaceUnit = true)
    {
        _canPlaceUnit = canPlaceUnit;
    }

    public static bool GetCanPlaceUnit()
    {
        return _canPlaceUnit;
    }
}