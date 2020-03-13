using System;
using UnityEngine;

/// <summary>
/// CollectibleSpawnerItem is a Serializable class that holds data about a spawnable item.
/// This includes a name, spawn chance and an item(GameObject) which is the item to be spawned/instatiated.
/// </summary>
[Serializable]
public class CollectibleSpawnerItem {

	[SerializeField]
	private string itemName;
	public string ItemName { get => itemName; private set => itemName = value; }

	[SerializeField]
	[Range(0, 100)]
	private int spawnChance;
	public int SpawnChance { get => spawnChance; private set => spawnChance = value; }

	[SerializeField]
	private Collectable item;
	public Collectable Item { get => item; private set => item = value; }

	public CollectibleSpawnerItem(string itemName, int spawnChance, Collectable item) {
		this.itemName = itemName;
		this.spawnChance = spawnChance;
		this.item = item;
	}

}