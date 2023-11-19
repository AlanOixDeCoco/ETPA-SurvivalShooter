using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStats : ICloneable
{
    [Header("Infos")] 
    public String name;
    public Material material;
    public float scale = 1f;

    [Header("Caracteristics")]
    [Tooltip("The detection sphere radius, in meters")] public float detectionRadius;
    [Tooltip("The detection sphere radius, when detected, in meters")] public float followRadius;
    public int health;
    public float speed;
    public float damage;
    public int difficulty;

    [Header("Loot")]
    [Range(0f, 1f)] public float lootProbability;
    public List<GameObject> loot;

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

[CreateAssetMenu(fileName = "EnemyStats", menuName = "EnemyStats")]
public class EnemyStatsSO : ScriptableObject
{
    public EnemyStats enemyStats;
}
