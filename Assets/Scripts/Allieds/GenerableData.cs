using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Generable 1", menuName = "Custom/Generable/Data")]
public class GenerableData : ScriptableObject
{
    [Header("Common")]
    public Generable.GenerableType gType;
    public GameObject associatedPrefab;
    public GameObject alternatePrefab;
    public Sprite sprite;
    [Range(0, 5)] public float creationTime;
    [Range(0, 5)] public float deployTime;

    [Header("Units")]
    public Unit.UnitType unityType;
    public ThinkingGenerable.AttackType attackType = ThinkingGenerable.AttackType.Ranged;
    public Generable.GenerableTarget targetType = Generable.GenerableTarget.Unit;
    [Range(0, 5)] public int cost;
    [Range(0, 5)] public float attackRate = 1f; 
    [Range(0, 5)] public float damagePerAttack = 2f; 
    [Range(0, 5)] public float attackRange = 1f;
    public float visualRange;
    [Range(0, 50)] public float hitPoints = 10f;
    public AudioClip attackClip, dieClip;

    //public float speed = 5f; 

}
