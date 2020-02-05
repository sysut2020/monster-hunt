using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class CollectibleSpawner : MonoBehaviour {
    private static CollectibleSpawner instance;

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

    // Can be loaded with prefabs or other game objects that can be spawned
    [SerializeField]
    private GameObject[] collectiblesToSpawn;

    [SerializeField]
    private int minimumSpawnItems = 1;

    [SerializeField]
    private int maximumSpawnItems = 5;

    /// <summary>
    /// Spawns a collectible at a specific location
    /// </summary>
    /// <param name="position">The spawn location</param>
    public void SpawnCollectible(Vector2 position) {
        int numberOfSpawnItems = Random.Range(minimumSpawnItems, maximumSpawnItems);

        for (int i = 0; i < numberOfSpawnItems; i++) {
            double randomNumber = Math.Ceiling(Random.value * 10);

            GameObject collectible = SetCollectibleType(randomNumber);
            if (collectible != null) {
                collectible.SetActive(true);
                collectible.tag = "Collectible";
                collectible.transform.position = position;
            }
        }
    }

    /// <summary>
    /// Set the collectible type based on a random number. This should be on chanse
    /// </summary>
    /// <param name="randomNumber">The number corresponding to type</param>
    /// <returns>Random collectible</returns>
    private GameObject SetCollectibleType(double randomNumber) {
        GameObject collectible = null;

        switch (randomNumber) {
            case var n when (n <= 2):
                collectible = new GameObject(); // todo change to prefab
                collectible.name = "PowerUP";
                break;

            case var n when (n > 2 && n <= 8):
                collectible =
                    Instantiate(
                        collectiblesToSpawn[
                            0]); // taking the collectible from the inspector and instantiate it in the scene
                collectible.name = "Coin";
                Debug.Log("Spawning Coin", collectible);
                break;

            case var n when (n > 8 && n <= 11):
                string letter = SudoRandomLetterGenerator.Instance.GenerateLetter();

                collectible = new GameObject(); // todo change to prefab containing the letter

                collectible.name = "Letter " + letter;
                break;

            default:
                Debug.Log("Not spawning anything");
                break;
        }

        return collectible;
    }
}