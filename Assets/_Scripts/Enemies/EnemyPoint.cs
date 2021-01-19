using UnityEngine;

public enum Direction
{
    Vertical,
    Horizontal
}

public class EnemyPoint : MonoBehaviour
{
    public Direction direction;

    [SerializeField] private int id;
    public int ID => id;
}