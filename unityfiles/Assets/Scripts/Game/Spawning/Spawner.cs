using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for Spawning enemys.
/// 
/// </summary>
public class Spawner : Singleton<Spawner> {

    // TODO: Make spawner more flexible

    // -- editor -- //
    [Header("spawning methods")]
    [Tooltip("spawn on and one")]
    [SerializeField]
    private bool spawnSingle = false;

    [Tooltip("check if theere is space to spawn if not don't")]
    [SerializeField]
    private bool chekIfSpace = true;

    [Tooltip("Time betwen spawns")]
    [SerializeField]
    private float timeBetweenSpawns = 0;

    [Header("Enemy Types")]
    [Tooltip("the types of enemy to spawn")]
    [SerializeField]
    private List<GameObject> enemyTypes = new List<GameObject>();

    private List<GameObject> spawnPoints = new List<GameObject>();
    private readonly Timers spawnTimer = new Timers();

    // -- Public -- //

    /// <summary>
    /// Spawn a copy of the mob given to the spawner at all 
    /// of the spawners GO children locations
    /// </summary>
    public void SpawnOnAll() {
        spawnPoints = (List<GameObject>) WUGameObjects.GetGOChildren(this.gameObject);

        // Generata a random seed for random generator
        UnityEngine.Random.InitState(UnityEngine.Random.Range(0, 1000) + DateTime.UtcNow.Millisecond);
        foreach (GameObject item in spawnPoints) {
            var index = (int) UnityEngine.Random.Range(0, enemyTypes.Count);
            GameObject EnemyCopy = Instantiate(enemyTypes[index]);
            EnemyCopy.transform.rotation = item.transform.rotation;
            EnemyCopy.transform.position = item.transform.position;
            EnemyCopy.SetActive(true);
        }
    }

    // -- private -- // 

    /// <summary>
    /// Spawns a the enemy on index enemyIndex at the spawnpint
    /// with the spawnpoint index
    /// </summary>
    /// <param name="pointIndex">the index of the point to spawn</param>
    /// <param name="enemyIndex">the index of the enemy to spawn</param>
    private void spawnEnemyOnPoint(int pointIndex, int enemyIndex) {
        if (WUInteger.IsInRange(pointIndex, 0, this.spawnPoints.Count) && WUInteger.IsInRange(enemyIndex, 0, this.enemyTypes.Count)) {
            GameObject spawnPoint = this.spawnPoints[pointIndex];
            GameObject EnemyCopy = Instantiate(enemyTypes[enemyIndex]);
            EnemyCopy.transform.rotation = spawnPoint.transform.rotation;
            EnemyCopy.transform.position = spawnPoint.transform.position;
            EnemyCopy.SetActive(true);
        }
    }

    // -- unity -- // 

}