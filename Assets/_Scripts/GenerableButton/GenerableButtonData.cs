using UnityEngine;

[CreateAssetMenu(fileName = "Button 1", menuName = "Custom/Generable/Button") ]
public class GenerableButtonData : ScriptableObject
{
    [Header("Button graphics")]
    public Sprite sourceSprite;
    public Sprite highlightedSprite;
    public Sprite pressedSprite;
    public Sprite selectedSprite;
    public Sprite disableSprite;

    [Header("List of Generables")]
    public GenerableData[] generablesData; 
    public Vector2[] relativeOffsets;
}
