﻿using System.Collections.Generic;
using UnityEngine;

public class FactoryTracker : MonoBehaviour
{
    [Tooltip("Referencia a la Grid de la escena actual")]
    [SerializeField] private GameObject TileGrid;

    private static GameObject _grid;
    private static Factory _currentFactory;
    private static FactoryInteractive _currentInteractive;
    private static bool _canPlaceUnit;
    private static bool _canPlaceFactory;
    private static List<Tile> _activeTiles;
    private static GameObject _factoryToGenerate;

    void Start()
    {
        Tile.OnMouseClick += () => PlaceUnit();
        Tile.OnMouseClick += () => PlaceFactory();
        _activeTiles = new List<Tile>();
        _grid = TileGrid;
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
        if (_activeTiles.Contains(tile))
        {
            if (_currentFactory != null)
                StartCoroutine(_currentFactory.GenerateUnit(tile));
            _canPlaceUnit = false;
        }
    }

    private void PlaceFactory()
    {
        if (!_canPlaceFactory) return;
        RaycastHit2D hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Input.mousePosition),
            Vector2.zero,
            10.0f,
            LayerMask.GetMask("Tile")
        );
        if (hit)
        {
            Instantiate(_factoryToGenerate, hit.collider.transform.position, Quaternion.identity);
            _canPlaceFactory = false;
        }
    }

    public static void SetCurrentFactory(Factory factory, FactoryInteractive interactive)
    {
        if (_currentFactory)
        {
            _currentInteractive.LoseFocus();
            _activeTiles.Clear();
        }
        _canPlaceFactory = false;
        _currentFactory = factory;
        _currentInteractive = interactive;
    }

    public static void PrepareFactoryGeneration(GameObject factory)
    {
        _factoryToGenerate = factory;
        _canPlaceFactory = !_canPlaceFactory;
        _currentInteractive?.LoseFocus();
        ShowGrid(true);
    }

    public static void ShowGrid(bool show) => _grid.SetActive(show);

    public static bool CanPlaceUnit()
    {
        return (_canPlaceUnit && _currentFactory.GetCanGenerateUnit()) ? true : false;
    }

    public static void SetCanPlaceUnit(bool canPlace) => _canPlaceUnit = canPlace;

    public static bool GetCanPlaceFactory() => _canPlaceFactory;

    public static void AddActiveTile(Tile tile) => _activeTiles.Add(tile);

    public static List<Tile> GetActiveTiles() => _activeTiles;
}
