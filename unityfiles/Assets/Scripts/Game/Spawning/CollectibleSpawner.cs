using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class CollectibleSpawner : MonoBehaviour {
    
    [SerializeField]
    private Collectable coinCollectable;
    [SerializeField]
    private Collectable letterCollectable;
    [SerializeField]
    private Collectable powerUpCollectable;

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
        Enemy.EnemyKilledEvent += SpawnCollectible;
    }

    /// <summary>
    /// Subscribes to the relevant events for this class
    /// </summary>
    private void UnsubscribeFromEvents() {
        Enemy.EnemyKilledEvent -= SpawnCollectible;
    }

    /// <summary>
    /// Spawns a collectible at a specific location
    /// </summary>
    /// <param name="position">The spawn location</param>
    public void SpawnCollectible(object _, EnemyEventArgs args) {
        int numberOfSpawnItems = Random.Range(minimumSpawnItems, maximumSpawnItems);

        for (int i = 0; i < numberOfSpawnItems; i++) {
            GameObject collectible = GetCollectibleType(args);
            if (collectible != null) {
                collectible.SetActive(true);
                collectible.tag = "Collectible";
                collectible.transform.position = args.Position;
            }
        }
    }

    /// <summary>
    /// Get the collectible type based on a random number. This should be on chance
    /// </summary>
    /// <param name="randomNumber">The number corresponding to type</param>
    /// <returns>Random collectible</returns>
    private GameObject GetCollectibleType(EnemyEventArgs args) {
        GameObject collectible = null;

        EnemyType e = args.EnemyType;
        float a = e.CoinDropChance + e.LetterDropChance + e.PowerUpDropChance;
        double randomNumber = Random.Range(0, a);
        switch (randomNumber) {
            case var n when(n <= e.PowerUpDropChance):
                collectible = Instantiate(powerUpCollectable.gameObject);
            break;
            case var n when(n < (e.CoinDropChance + e.CoinDropChance)):
                collectible = Instantiate(coinCollectable.gameObject);
            break;
            default:
                collectible = Instantiate(letterCollectable.gameObject);
                break;

        }

        return collectible;
    }
}