using System;
using UnityEngine;

/// <summary>
/// CollectibleSpawnerItem is a Serializable class that holds data about a spawnable item.
/// This includes a name, spawn chance and an item(GameObject) which is the item to be spawned/instantiated.
/// </summary>
[Serializable]
public class CollectibleSpawnerItem {

    public CollectibleSpawnerItem(int spawnChance, Collectible item) {
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
    private Collectible item;

    /// <summary>
    /// Chance for being spawned
    /// </summary>
    /// <value>Value between 0 and 100</value>
    public int SpawnChance { get => spawnChance; private set => spawnChance = value; }

    /// <summary>
    /// Collectible spawner item
    /// </summary>
    /// <value>spawner item</value>
    public Collectible Item { get => item; private set => item = value; }

}