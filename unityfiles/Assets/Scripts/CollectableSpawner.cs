using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour {
    [SerializeField] private int minimumSpawnItems = 0;
    private int maximumSpawnItems = 5;

    // Collectable to spawn
    private GameObject collectable = null;

    public int MaximumSpawnItems {
        get { return maximumSpawnItems; }
        set { maximumSpawnItems = value; }
    }

    public int MinimumSpawnItems {
        get { return minimumSpawnItems; }
        set { minimumSpawnItems = value; }
    }

    public GameObject Prefab {
        get { return prefab; }
        set { prefab = value; }
    }

    public void SpawnCollectable() {
        int spawns = Random.Range(minimumSpawnItems, maximumSpawnItems);
        for (int i = 0; i < spawns; i++)
        {
            // todo spawn collectable
        }
    }
}