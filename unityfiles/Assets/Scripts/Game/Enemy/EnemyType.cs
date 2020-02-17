using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describes a type of Enemy
/// </summary>
[CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemyType : ScriptableObject {

    [Header ("Enemy stats")]
    [Tooltip ("the amount of heath the enemy has")]
    [SerializeField]
    private int health;

    [Tooltip ("The enemy movement speed")]
    [SerializeField]
    private float speed;


    [Header ("Enemy drop Chances")]

    [Tooltip ("Letter drop chance")]
    [SerializeField]
    private float letterDropChance;

    [Tooltip ("Coin drop chance")]
    [SerializeField]
    private float coinDropChance;

    [Tooltip ("PU drop chance")]
    [SerializeField]
    private float powerUpDropChance;

    public int Health { get => health;}
    public float Speed { get => speed;}
    public float LetterDropChance { get => letterDropChance;}
    public float CoinDropChance { get => coinDropChance;}
    public float PowerUpDropChance { get => powerUpDropChance;}
}