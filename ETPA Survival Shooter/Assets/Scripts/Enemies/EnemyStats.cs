using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTypes
{
    blue,
    green,
    red
}

[System.Serializable]
public class EnemyStats : ICloneable
{
    public Material material;
    public EnemyTypes type;
    public int maxHealth;
    public int health;
    public float speed;

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

[CreateAssetMenu]
public class EnemyStatsSO : ScriptableObject
{
    public EnemyStats enemyStats;
}
