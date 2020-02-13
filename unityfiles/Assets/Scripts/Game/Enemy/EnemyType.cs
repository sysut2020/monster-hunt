using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describes a type of Enemy
/// </summary>
[CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemyType : ScriptableObject {
    public int health;

    public float speed;

    public float letterDropChance;
    public float coinDropChance;
    public float powerUpDropChance;


}