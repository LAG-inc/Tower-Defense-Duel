using UnityEngine;


[CreateAssetMenu(fileName = "Enemy", menuName = "Custom/Enemy", order = 0)]
public class EnemyScriptable : ScriptableObject
{
    public Sprite enemySprite;
    public float life;
    public float damage;
}