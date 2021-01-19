using UnityEngine;

public class FactoryInteractive : MonoBehaviour
{
    [Header("Components")]
    [SerializeField, Tooltip("Referencia a la fabrica del mismo prefab")]
    private Factory factory;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Canvas ui;

    [Header("Factory range")]
    [SerializeField, Range(2.0f, 10.0f)] private float _rangeX = 2.0f;
    [SerializeField, Range(2.0f, 10.0f)] private float _rangeY = 2.0f;

    [Header("Colors")]
    [SerializeField] private Color focusColor;

    private void OnMouseDown()
    {
        //lose focus when click again in the same factory
        if (ui.gameObject.activeSelf)
            LoseFocus();
        else
            GainFocus();
    }

    public void LoseFocus()
    {
        spriteRenderer.color = Color.white;
        ui.gameObject.SetActive(false);
        FactoryTracker.SetCanPlaceUnit(false);
        ShowArea(false);
    }

    private void GainFocus()
    {
        FactoryTracker.SetCurrentFactory(factory, this);
        spriteRenderer.color = focusColor;
        ui.gameObject.SetActive(true);
        ShowArea(true);
    }

    private void GetTilesInArea()
    {
        RaycastHit2D[] tiles = Physics2D.BoxCastAll(
            transform.position,
            new Vector3(_rangeX, _rangeY),
            0.0f,
            Vector2.zero,
            0.0f,
            LayerMask.GetMask("Tile"));
        foreach (RaycastHit2D tile in tiles)
            FactoryTracker.AddActiveTile(tile.collider.GetComponent<Tile>());
    }

    private void ShowArea(bool show)
    {
        GetTilesInArea();
        foreach (Tile tile in FactoryTracker.GetActiveTiles())
        {
            if (show)
                tile.ChangeState();
            else
                tile.ResetState();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.15f);
        Gizmos.DrawCube(transform.position, new Vector3(_rangeX, _rangeY));
    }
}
