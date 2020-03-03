using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describes a type of Enemy
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemyType : ScriptableObject {

    [Header("Enemy stats")]
    [Tooltip("the amount of heath the enemy has")]
    [SerializeField]
    private int health;

    [Tooltip("The enemy movement speed")]
    [SerializeField]
    private float speed;

    [Header("Spawnable items from enemy")]

    [SerializeField]
    private List<CollectibleSpawnerItem> collectibleItems;

    public int Health { get => health; }
    public float Speed { get => speed; }
    public List<CollectibleSpawnerItem> CollectibleItems { get => collectibleItems; }
}