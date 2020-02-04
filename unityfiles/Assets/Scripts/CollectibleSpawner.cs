using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class CollectibleSpawner : MonoBehaviour {
    private static CollectibleSpawner _instance;

    public static CollectibleSpawner Instance => _instance;


    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public class Collectible : Object {
        public GameObject gameObject;
        public float spawnChanseWeight;
    }
    
    // enum Collectibles {
    //     PowerUp,
    //     Coin,
    //     Letter
    // }

    // Can be loaded with prefabs or other game objects that can be spawned
    [SerializeField]
    private GameObject[] collectiblesToSpawn;

    [SerializeField]
    private int minimumSpawnItems = 1;

    [SerializeField]
    private int maximumSpawnItems = 5;

    public void SpawnCollectible(Vector2 position) {
        Debug.Log("Spawn position should be" + position);
        int numberOfSpawnItems = Random.Range(minimumSpawnItems, maximumSpawnItems);

        for (int i = 0; i < numberOfSpawnItems; i++) {
            double randomNumber = Math.Ceiling(Random.value * 10);

            GameObject collectible = SetCollectibleType(randomNumber);
            // todo spawn collectable
            collectible.transform.position = position;
        }
    }

    private GameObject SetCollectibleType(double randomNumber) {
        GameObject collectible;

        Debug.Log("The random number is " + randomNumber);

        switch (randomNumber) {
            case var n when (n <= 2):
                Debug.Log("I'm less than 2");
                collectible = new GameObject(); // todo change to prefab
                collectible.name = "PowerUP";
                break;

            case var n when (n > 2 && n <= 8):
                Debug.Log("I'm between 2 and 8");
                Debug.Log("spawning coin");
                var test = collectiblesToSpawn[0];
                collectible = Instantiate(test);
                collectible.name = "Coin";
                break;

            case var n when (n > 8 && n <= 11):
                Debug.Log("I'm between 8 and 10");
                string letter = SudoRandomLetterGenerator.Instance.GenerateLetter();

                collectible = new GameObject(); // todo change to prefab containing the letter

                collectible.name = "Letter " + letter;
                break;

            default:
                Debug.Log("I'm out of range");
                collectible = new GameObject(); // todo change to prefab
                break;
        }

        collectible.SetActive(true);
        collectible.tag = "Collectible";
        return collectible;
    }
}