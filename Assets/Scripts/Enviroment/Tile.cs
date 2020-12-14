using UnityEngine;

[RequireComponent(typeof(SpriteRenderer)), DisallowMultipleComponent]
public class Tile : MonoBehaviour
{
    //Event invoked when @OnMouseUp is Invoked, designed to instance objs on click.
    public static event EventManager.VoidEvent OnMouseClick;
    private SpriteRenderer _sprite;
    private Color _initialColor;
    private Color _onMouseOverColor;

    private static bool _isOverTile;

    // Indicates if the tile is empty or not
    private bool _isEmpty = true;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _initialColor = _sprite.color;
        _onMouseOverColor = new Color(_initialColor.r, _initialColor.g, _initialColor.b, 0.2f);
    }

    private void OnMouseOver()
    {
        _sprite.color = _onMouseOverColor;
    }

    private void OnMouseEnter()
    {
        _isOverTile = true;
    }

    private void OnMouseExit()
    {
        _isOverTile = false;
        _sprite.color = _initialColor;
    }

    // In a future maybe we'll need to use raycast instead
    private void OnMouseUp()
    {
        if (!_isOverTile || !_isEmpty || !FactoryTracker.GetCanPlaceUnit())
            return;
        OnMouseClick?.Invoke();
        _isEmpty = false;
        Debug.Log("Mouse Up");
    }

    private void OnDestroy()
    {
        OnMouseClick = null;
    }
}