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
    public Generable.Faction unitFaction;
    public ThinkingGenerable.AttackType attackType = ThinkingGenerable.AttackType.Ranged;
    public Generable.GenerableTarget targetType = Generable.GenerableTarget.Unit;
    [Range(0, 5)] public int cost;
    [Range(0, 100)] public float attackRate = 1f; 
    [Range(0, 5)] public float damagePerAttack = 2f; 
    [Range(10, 30)] public float attackRange = 10f;
    [Range(0, 50)] public float hitPoints = 10f;
    public AudioClip attackClip, dieClip;

    [Header("Allies")]
    public Allied.AType aType;
    [Range(0, 5)] public float deployTime;    

    [Header("Enemies")]
    public Enemy.EType eType;
    public float speed;
    public int points;

}
