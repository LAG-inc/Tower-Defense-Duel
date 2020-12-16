using UnityEngine;


[CreateAssetMenu(fileName = "Enemy", menuName = "Enemies", order = 0)]
public class EnemyScriptable : ScriptableObject
{
    public Sprite enemySprite;
    public float life;
    public float damage;
}