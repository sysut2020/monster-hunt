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

	[Tooltip("How much damage this dinosaur has")]
	[SerializeField]
	private int damage;

	[Tooltip("The speed when patroling")]
	[SerializeField]
	private float patrolSpeed;

	[Tooltip("The speed when chasing/charging")]
	[SerializeField]
	private float chaseSpeed;

	[Tooltip("How far it can see")]
	[SerializeField]
	private float visionLength;

	[Header("Spawnable items from enemy")]

	[SerializeField]
	private List<CollectibleSpawnerItem> collectibleItems;

	public int Health { get => health; }
	public int Damage { get => damage; }
	public float PatrolSpeed { get => patrolSpeed; }
	public float ChaseSpeed { get => chaseSpeed; }
	public float VisionLength { get => visionLength; }
	public List<CollectibleSpawnerItem> CollectibleItems { get => collectibleItems; }
}