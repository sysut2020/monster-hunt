using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -- properties -- //
// -- public -- //
// -- private -- // 

/// <summary>
/// Responsible for Spawning enemys.
/// 
/// </summary>
public class Spawner : MonoBehaviour {

    // -- editor -- //
    [Header ("spawning methods")]
    [Tooltip ("spawn on and one")]
    [SerializeField]
    private bool spawnSingle = false;

    [Tooltip ("check if theere is space to spawn if not don't")]
    [SerializeField]
    private bool chekIfSpace = true;

    [Tooltip ("Time betwen spawns")]
    [SerializeField]
    private float timeBetweenSpawns = 0;

    [Header ("Enemy Types")]
    [Tooltip ("the types of enemy to spawn")]
    [SerializeField]
    private List<GameObject> enemyTypes = new List<GameObject> ();

    // -- internal -- //

    private static Spawner instance;

    public static Spawner Instance{
        get {
            if (instance == null) {
                instance = new Spawner();
            }

            return instance;
        }
    }

    private List<GameObject> spawnPoints = new List<GameObject> ();
    private readonly Timers spawnTimer = new Timers();

    // -- Public -- //
    public void SpawnOnAll(){
        print("SPAWNING");
        spawnPoints = (List<GameObject>) WUGameObjects.GetGOChildren(this.gameObject);

        foreach (GameObject item in spawnPoints) {
            GameObject EnemyCopy = Instantiate (enemyTypes[0]);
            EnemyCopy.transform.rotation = item.transform.rotation;
            EnemyCopy.transform.position = item.transform.position;
            EnemyCopy.SetActive (true);
        }
    }

    // -- private -- // 

    /// <summary>
    /// Spawns a the enemy on index enemyIndex at the spawnpint
    /// with the spawnpoint index
    /// </summary>
    /// <param name="pointIndex">the index of the point to spawn</param>
    /// <param name="enemyIndex">the index of the enemy to spawn</param>
    private void spawnEnemyOnPoint(int pointIndex, int enemyIndex){
        if (WUInteger.IsInRange(pointIndex, 0, this.spawnPoints.Count) && WUInteger.IsInRange(enemyIndex, 0, this.enemyTypes.Count)){
            GameObject spawnPoint = this.spawnPoints[pointIndex];
            GameObject EnemyCopy = Instantiate (enemyTypes[enemyIndex]);
            EnemyCopy.transform.rotation = spawnPoint.transform.rotation;
            EnemyCopy.transform.position = spawnPoint.transform.position;
            EnemyCopy.SetActive (true);
        }
    }
    


    // -- unity -- // 
    private void Awake(){
        instance = this;
    }
}