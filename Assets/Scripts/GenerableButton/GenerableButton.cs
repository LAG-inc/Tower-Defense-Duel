using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GenerableButton : MonoBehaviour, IPointerUpHandler
{
    public UnityAction<GenerableButtonData> OnMouseUp;

    [SerializeField] private GenerableButtonData buttonData;
    private Button buttonComponent;
    private Image imageComponent;

    private void Start()
    {
        buttonComponent = GetComponent<Button>();
        imageComponent = GetComponent<Image>();
        Activate();
    }

    public void OnPointerUp(PointerEventData pointerEvent)
    {
        if(OnMouseUp != null)
        {
            OnMouseUp(buttonData);
        }
    }

    public void Activate()
    {
        imageComponent.sprite = buttonData.sourceSprite;

        buttonComponent.transition = Selectable.Transition.SpriteSwap;

        SpriteState state = new SpriteState();
        state = buttonComponent.spriteState;
        state.highlightedSprite = buttonData.highlightedSprite;
        state.pressedSprite = buttonData.pressedSprite;
        state.selectedSprite= buttonData.selectedSprite;
        state.disabledSprite = buttonData.disableSprite;
        buttonComponent.spriteState = state;
    }

    public GenerableButtonData GetButtonData()
    {
        return buttonData;
    }

    public void DisableButton()
    {
        buttonComponent.interactable = false;
    }

    public void EnableButton()
    {
        buttonComponent.interactable = true;
    }

    public bool IsInteractable()
    {
        return buttonComponent.interactable;
    }
}
