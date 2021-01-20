using UnityEngine;

[RequireComponent(typeof(SpriteRenderer)), DisallowMultipleComponent]
public class Tile : MonoBehaviour
{
    //Event invoked when @OnMouseUp is Invoked, designed to instance objs on click.
    public static event EventManager.VoidEvent OnMouseClick;
    private SpriteRenderer _sprite;
    private Color _initialColor;
    private Color _onMouseOverColor;

    [Header("Options")]
    // Indicates if the tile is empty or not
    [SerializeField] private bool _isEmpty = true;

    [Header("Colors")]
    [SerializeField] private Color _showAvailableColor;
    [SerializeField] private Color _showUnavailableColor;

    [Header("Components")]
    [SerializeField] private Collider2D _collider;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _initialColor = _sprite.color;
        _onMouseOverColor = new Color(_initialColor.r, _initialColor.g, _initialColor.b, 0.2f);
    }

    private void OnMouseOver()
    {
        _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 0.8f);
    }

    private void OnMouseExit()
    {
        _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 0.5f);
    }

    // In a future maybe we'll need to use raycast instead
    private void OnMouseUp()
    {
        if (!_isEmpty) return;
        // Place units
        if (FactoryTracker.CanPlaceUnit())
        {
            OnMouseClick?.Invoke();
            _isEmpty = false;
            ChangeState();
        }

        // Place factories
        if (FactoryTracker.GetCanPlaceFactory())
        {
            OnMouseClick?.Invoke();
            _isEmpty = false;
            _collider.enabled = false;
        }
    }

    private void OnDestroy()
    {
        OnMouseClick = null;
    }

    public void ChangeState()
    {
        if(!FactoryTracker.GetActiveTiles().Contains(this)) return;
        if (_isEmpty || FactoryTracker.CanPlaceUnit())
            _sprite.color = _showAvailableColor;
        else
            _sprite.color = _showUnavailableColor;
    }

    internal void ResetState() => _sprite.color = _initialColor;
}
