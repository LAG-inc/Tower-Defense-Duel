using UnityEngine;
using UnityEngine.Events;

public class Generable : MonoBehaviour
{
    public GenerableType gType;
    [HideInInspector] public GenerableTarget targetType;
    [HideInInspector] public Faction faction;
    [HideInInspector] public AudioClip dieAudioClip;

    public UnityAction<Generable> OnDie;

    public enum GenerableType
    {
        Unit,
        Resource
    }
    public enum GenerableTarget
    {
        OnlyBuildings,
        Unit,
        Both,
        None,
    }
    public enum Faction
    {
        Player, 
        Opponent,
        None,
    }

}
