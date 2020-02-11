using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class CollectibleSpawner : MonoBehaviour {
    // Can be loaded with prefabs or other game objects that can be spawned
    // [SerializeField]
    // private GameObject[] collectiblesToSpawn;

    [SerializeField]
    private GameObject coinCollectable;
    [SerializeField]
    private LetterController letterCollectable;
    [SerializeField]
    private GameObject powerUpCollectable;

    [SerializeField]
    private int minimumSpawnItems = 1;

    [SerializeField]
    private int maximumSpawnItems = 5;

    private static CollectibleSpawner instance;

    // Singleton pattern implementation
    public static CollectibleSpawner Instance {
        get {
            if (instance == null) {
                GameObject gameObject = new GameObject("CollectibleSpawner");
                gameObject.AddComponent<CollectibleSpawner>();
            }

            return instance;
        }
    }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Spawns a collectible at a specific location
    /// </summary>
    /// <param name="position">The spawn location</param>
    public void SpawnCollectible(Vector2 position) {
        int numberOfSpawnItems = Random.Range(minimumSpawnItems, maximumSpawnItems);

        for (int i = 0; i < numberOfSpawnItems; i++) {
            double randomNumber = Math.Ceiling(Random.value * 10);

            GameObject collectible = GetCollectibleType(randomNumber);
            if (collectible != null) {
                collectible.SetActive(true);
                collectible.tag = "Collectible";
                collectible.transform.position = position;
            }
        }
    }

    /// <summary>
    /// Get the collectible type based on a random number. This should be on chance
    /// </summary>
    /// <param name="randomNumber">The number corresponding to type</param>
    /// <returns>Random collectible</returns>
    private GameObject GetCollectibleType(double randomNumber) {
        GameObject collectible = null;

        switch (randomNumber) {
            case var n when(n <= 2):
                collectible = Instantiate(powerUpCollectable);
            collectible.name = "PowerUP";
            break;

            case var n when(n > 2 && n <= 8):
                // taking the collectible from the inspector and instantiate it in the scene
                collectible = Instantiate(coinCollectable);
            collectible.name = "Coin";
            break;

            case var n when(n > 8 && n <= 11):
                string letter = SudoRandomLetterGenerator.Instance.GenerateLetter();
            LetterController le = Instantiate(letterCollectable);
            collectible = le.gameObject;
            le.SetLetter(letter);
            collectible.name = "Letter " + letter;
            break;

            default:
                // no default behavior
                break;
        }

        return collectible;
    }
}