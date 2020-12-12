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


    private void OnMouseUp()
    {
        if (!_isOverTile) return;
        OnMouseClick?.Invoke();
        Debug.Log("Mouse Up");
    }

    private void OnDestroy()
    {
        OnMouseClick = null;
    }
}