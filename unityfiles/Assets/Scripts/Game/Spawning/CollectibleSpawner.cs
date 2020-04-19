using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

/// <summary>
/// Responsible for selecting and spawning collectables items provided in a 
/// list of <see cref="CollectibleSpawnerItems"/> which all have a spawn chance.
/// </summary>
public class CollectibleSpawner : MonoBehaviour {

    [SerializeField]
    private int minimumSpawnItems = 1;

    [SerializeField]
    private int maximumSpawnItems = 5;

    private void Awake() {
        SubscribeToEvents();

    }
    private void OnDestroy() {
        UnsubscribeFromEvents();
    }

    /// <summary>
    /// Subscribes to the relevant events for this class
    /// </summary>
    private void SubscribeToEvents() {
        Enemy.EnemyKilledEvent += TrySpawnCollectable;
    }

    /// <summary>
    /// Subscribes to the relevant events for this class
    /// </summary>
    private void UnsubscribeFromEvents() {
        Enemy.EnemyKilledEvent -= TrySpawnCollectable;
    }

    /// <summary>
    /// Tries to spawn collectables from the items provided in the list of 
    /// items from args. It will spawn from minimumSpawnItems to maximumSpawnItems
    /// set in the inspector.
    /// </summary>
    public void TrySpawnCollectable(object _, EnemyEventArgs args) {
        int numberOfItemsToSpawn = Random.Range(minimumSpawnItems, maximumSpawnItems);
        var collectiblesTable = args.EnemyType.CollectibleItems;
        int totalSpawnWeight = CalculateTotalSpawnWeight(collectiblesTable);

        for (int i = 0; i < numberOfItemsToSpawn; i++) {
            var collectableToSpawn = this.TryGetSpawnable(collectiblesTable, totalSpawnWeight);
            TryCreateCollectable(collectableToSpawn, args.Position);
        }
    }

    private void TryCreateCollectable(CollectibleSpawnerItem item, Vector3 position) {
        try {
            var collectible = Instantiate(item.Item.gameObject);
            collectible.transform.position = position;
        } catch (System.NullReferenceException) {
            Debug.LogWarning("Null reference when instatiating collectable");
        }
    }

    private CollectibleSpawnerItem TryGetSpawnable(List<CollectibleSpawnerItem> spawnerItems, int totalSpawnWeight) {
        int weightedSpawnChance = Random.Range(0, totalSpawnWeight);

        // Loops over the spawnerItems list and returns the first element from the
        // spawnerItems list that satisfies the if condition. Else it will just continue
        // until the list is empty. On each step where there is no match, also reduce the 
        // weightedSpawnChance, in hope to catch an item that satisfies the if condition.
        // https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.find?view=netframework-4.8
        return spawnerItems.Find(collectableItem => {
            if (weightedSpawnChance <= collectableItem.SpawnChance) { return true; }
            weightedSpawnChance -= collectableItem.SpawnChance;
            return false;
        });
    }

    private int CalculateTotalSpawnWeight(List<CollectibleSpawnerItem> spawnerItems) {
        int weight = 0;
        spawnerItems.ForEach(collectableItem => weight += collectableItem.SpawnChance);
        return weight;
    }

}