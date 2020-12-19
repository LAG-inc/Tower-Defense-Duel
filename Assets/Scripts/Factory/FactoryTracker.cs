using System.Collections.Generic;
using UnityEngine;

public class FactoryTracker : MonoBehaviour
{
    private static Factory _currentFactory;
    private static FactoryInteractive _currentInteractive;
    private static bool _canPlaceUnit;
    private static List<Tile> _activeTiles;

    void Start()
    {
        Tile.OnMouseClick += () => PlaceUnit();
        _activeTiles = new List<Tile>();
    }

    private void PlaceUnit()
    {
        if (!_canPlaceUnit) return;
        RaycastHit2D hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Input.mousePosition),
            Vector2.zero,
            10.0f,
            LayerMask.GetMask("Tile"));
        Tile tile = hit.collider.GetComponent<Tile>();
        if (FactoryTracker.GetActiveTiles().Contains(tile))
        {
            // TODO: pass game object to have the reference
            _currentFactory?.GenerateUnit(hit.collider.transform.position);
            _canPlaceUnit = false;
        }
    }

    public static void SetCurrentFactory(Factory factory, FactoryInteractive interactive)
    {
        if (_currentFactory)
        {
            _currentInteractive.LoseFocus();
            _activeTiles.Clear();
        }
        _currentFactory = factory;
        _currentInteractive = interactive;
    }

    public static bool CanPlaceUnit()
    {
        return (_canPlaceUnit && _currentFactory.GetCanGenerateUnit()) ? true : false;
    }

    public static void SetCanPlaceUnit(bool canPlaceUnit) => _canPlaceUnit = canPlaceUnit;

    public static bool GetCanPlaceUnit() => _canPlaceUnit;

    public static void AddActiveTile(Tile tile) => _activeTiles.Add(tile);

    public static List<Tile> GetActiveTiles() => _activeTiles;
}
