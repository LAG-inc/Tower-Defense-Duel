using UnityEngine;


[CreateAssetMenu(fileName = "WaveScriptable", menuName = "Custom/Waves")]
public class WavesScriptable : ScriptableObject
{
    public float enemyVelocity;
    public int enemiesInPhase;
}