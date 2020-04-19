using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Responsible for Spawning enitites into the world.
/// How to use:
/// Spawn points are added by adding child GameObjects with SpawnPoint script
/// attached.
/// 
/// </summary>
public class EntitySpawner : Singleton<EntitySpawner> {

    // Time before trying to spawn a new entity
    private readonly int TRY_SPAWN_TIME = 10000; // Milliseconds

    [Tooltip ("Time between spawns")]
    [SerializeField]
    private float timeBetweenSpawns = 0; // Unused for now to be implemented

    [Header ("Spawnable entities")]
    [SerializeField]
    private List<GameObject> entities = new List<GameObject> ();

    // All spawnpoints found as childs.
    private List<SpawnPoint> spawnPoints = new List<SpawnPoint> ();

    public int MaxSpawns { get; set; } = 0;

    private int Spawned { get; set; } = 0;

    private int RestToSpawn { get; set; } = 0;

    private Camera MainCamera { get; set; }

    private WUTimers spawnTimer;

    private string spawnTimerId;

    private void Awake () {
        MainCamera = Camera.main;
        Enemy.EnemyKilledEvent += CallbackOnEnemyKilled;
        spawnPoints = GetComponentsInChildren<SpawnPoint> ().ToList ();
    }

    private void Start () {
        this.SetupSpawnTimer ();
    }

    private void OnDestroy () {
        Enemy.EnemyKilledEvent -= CallbackOnEnemyKilled;
    }

    private void SetupSpawnTimer () {
        spawnTimer = new WUTimers ();
        spawnTimerId = spawnTimer.RollingUID;
        spawnTimer.Set (spawnTimerId, TRY_SPAWN_TIME);
    }

    /// <summary>
    /// Initialize the spwaner by trying to spawn provided amount of entities.
    /// </summary>
    /// <param name="amountToSpawn"></param>
    public void Init (int amountToSpawn) {
        this.TrySpawn (amountToSpawn);
    }

    /// <summary>
    /// Spawn an entity on each spawn point.
    /// </summary>
    public void SpawnOnAll () {
        SelectNonVisibleSpawnpoints ();
        foreach (var point in spawnPoints) {
            point.Spawn (GetRandomEntity ());
        }
    }

    private void CallbackOnEnemyKilled (object _, EnemyEventArgs args) {
        if (this.Spawned == MaxSpawns) return; // Dont proceed of we met max spawns
        this.RestToSpawn++;
    }

    /// <summary>
    /// Tries to spawn X entities on random spawn points outside camera vision.
    /// If it cant spawn all entities, the rest is cached.
    /// </summary>
    /// <param name="sizeOfSpawn"></param>
    private void TrySpawn (int sizeOfSpawn) {
        if (Spawned > MaxSpawns) return; // Skip if maximum spawns is reached

        var possibleSpawnPoints = SelectNonVisibleSpawnpoints ();
        possibleSpawnPoints.Shuffle ();

        int pointIndex = 0;
        while (0 < sizeOfSpawn && pointIndex < possibleSpawnPoints.Count) {
            var point = possibleSpawnPoints[pointIndex];
            if (point.IsAvailable) {
                point.Spawn (GetRandomEntity ());
                sizeOfSpawn--;
                Spawned++;
            }
            pointIndex++;
        }
        RestToSpawn = sizeOfSpawn;
    }

    private void Update () {
        if (0 < RestToSpawn) {
            TrySpawn (RestToSpawn);
        }
        if (spawnTimer.Done (spawnTimerId, true)) {
            TrySpawn (1);
        }

    }

    /// <summary>
    /// Finds all spawnpoints not visible to the main camera
    /// </summary>
    /// <returns></returns>
    private List<SpawnPoint> SelectNonVisibleSpawnpoints () {
        return spawnPoints.FindAll (point => !CameraUtil.IsTargetVisible (MainCamera, point.gameObject));
    }

    /// <summary>
    /// Selects a random enity from the avaiable enities and returns it.
    /// if the list is empty, return null
    /// </summary>
    /// <returns>random selected entity or null if list is empty</returns>
    private GameObject GetRandomEntity () {
        var index = UnityEngine.Random.Range (0, entities.Count);
        GameObject selected = null;
        if (this.entities.Count > 0) {
            selected = this.entities[index];
        }
        return selected;
    }
}