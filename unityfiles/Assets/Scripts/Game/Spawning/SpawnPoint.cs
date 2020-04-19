using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Spawn point is a point in the world which is available as spawn point
/// for the EnitySpawner.
/// </summary>
public class SpawnPoint : MonoBehaviour {

    /// <summary>
    /// Flag to check if the spawn poin is available
    /// </summary>
    /// <value></value>
    public bool IsAvailable { get; private set; } = true;

    /// <summary>
    /// Tries to spawn an entity at the spawn points position if the 
    /// SpawnPoint is available. If the enity provided is null, do nothing.
    /// </summary>
    /// <param name="toSpawn"></param>
    public void Spawn(GameObject toSpawn) {
        if (!this.IsAvailable || toSpawn == null) {
            return;
        }

        GameObject EnemyCopy = Instantiate(toSpawn);

        // Add callback notifier, to get a notification when the spawned object,
        // is destroyed, so we can set the point to available again.
        var notifier = EnemyCopy.AddComponent<DestroyedCallbackNotifyer>();
        notifier.SetCallback(() => this.IsAvailable = true);
        this.IsAvailable = false;

        EnemyCopy.transform.rotation = this.transform.rotation;
        EnemyCopy.transform.position = this.transform.position;
        EnemyCopy.SetActive(true);
    }
}