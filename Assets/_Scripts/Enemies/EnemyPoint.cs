using UnityEngine;

public enum Direction
{
    Vertical,
    Horizontal
}

public class EnemyPoint : MonoBehaviour
{
    [SerializeField] private int id;
    public int ID => id;
}