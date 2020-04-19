using System;
using UnityEngine;

/// <summary>
/// CollectibleSpawnerItem is a Serializable class that holds data about a spawnable item.
/// This includes a name, spawn chance and an item(GameObject) which is the item to be spawned/instatiated.
/// </summary>
[Serializable]
public class CollectibleSpawnerItem {

	public CollectibleSpawnerItem(int spawnChance, Collectable item) {
		this.itemName = item.Name;
		this.spawnChance = spawnChance;
		this.item = item;
	}

	/// <summary>
	/// Higher value higher chance of been spawned
	/// </summary>
	[SerializeField]
	[Range(0, 100)]
	[Tooltip("Higher values means more likely to get spawned")]
	private int spawnChance;

	[SerializeField]
	private Collectable item;
	
	private string itemName;

	public string ItemName { get => itemName; private set => itemName = value; }
	public int SpawnChance { get => spawnChance; private set => spawnChance = value; }
	public Collectable Item { get => item; private set => item = value; }

	

}