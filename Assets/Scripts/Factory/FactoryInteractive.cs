using UnityEngine;

public class FactoryInteractive : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia a la fabrica del mismo prefab")]
    private Factory factory;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Canvas ui;

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
    }

    private void GainFocus()
    {
        FactoryTracker.SetCurrentFactory(factory, this);
        spriteRenderer.color = focusColor;
        ui.gameObject.SetActive(true);
    }
}