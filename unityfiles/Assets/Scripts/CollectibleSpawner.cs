using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CollectibleSpawner : MonoBehaviour {
    enum Collectibles {
        PowerUp,
        Coin,
        Letter
    }

    [SerializeField]
    private int minimumSpawnItems = 0;

    [SerializeField]
    private int maximumSpawnItems = 5;

    // Collectible to spawn
    [SerializeField]
    private GameObject collectible;

    public int MinimumSpawnItems {
        get { return minimumSpawnItems; }
        set { minimumSpawnItems = value; }
    }

    public int MaximumSpawnItems {
        get { return maximumSpawnItems; }
        set { maximumSpawnItems = value; }
    }

    public GameObject Collectible {
        get { return collectible; }
        set { collectible = value; }
    }

    public void SpawnCollectible(Vector2 position) {
        int numberOfSpawnItems = Random.Range(minimumSpawnItems, maximumSpawnItems);
        for (int i = 0; i < numberOfSpawnItems; i++) {
            double randomNumber = Math.Ceiling(Random.value * 10);

            collectible = SetCollectibleType(randomNumber);
            // todo spawn collectable
            Instantiate(collectible, position, Quaternion.identity);
        }
    }

    private GameObject SetCollectibleType(double randomNumber) {
        GameObject collectible = new GameObject();
        collectible.tag = "Collectible";
        

        switch (randomNumber) {
            case var n when (n < 2):
                Debug.Log("I'm less than 2");
                collectible.name = "PowerUP";
                break;

            case var n when (n > 2 && n < 8):
                Debug.Log("I'm between 2 and 8");
                break;

            case var n when (n > 8 && n <= 10):
                Debug.Log("I'm between 8 and 10");
                break;

            default:
                Debug.Log("I'm out of range");
                break;
        }

        return collectible;
    }
}