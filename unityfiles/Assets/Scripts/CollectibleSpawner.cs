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

    // Can be loaded with prefabs or other game objects that can be spawned
    [SerializeField]
    private GameObject[] collectiblesToSpawn;

    [SerializeField]
    private int minimumSpawnItems = 1;

    [SerializeField]
    private int maximumSpawnItems = 5;

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

    private GameObject SetCollectibleType(double randomNumber) {
        GameObject collectible = null;

        switch (randomNumber) {
            case var n when (n <= 2):
                collectible = new GameObject(); // todo change to prefab
                collectible.name = "PowerUP";
                break;

            case var n when (n > 2 && n <= 8):
                var test = collectiblesToSpawn[0];
                collectible = Instantiate(test);
                collectible.name = "Coin";
                Debug.Log("Spawning Coin", collectible);
                break;

            case var n when (n > 8 && n <= 11):
                string letter = SudoRandomLetterGenerator.Instance.GenerateLetter();

                collectible = new GameObject(); // todo change to prefab containing the letter

                collectible.name = "Letter " + letter;
                break;

            default:
                Debug.Log("Not spawining any thing");
                break;
        }

        return collectible;
    }
}