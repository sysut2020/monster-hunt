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

    private List<GameObject> spawnPoints = new List<GameObject> ();
    private Timers spawnTimer = new Timers();

    // -- private -- // 

    //private void spawnEnemyOnPoint(int pointIndex, int enemyIndex){}
    


    // -- unity -- // 
    void Start () {

        spawnPoints = (List<GameObject>) WUGameObjects.GetGOChildren(this.gameObject);

        foreach (GameObject item in spawnPoints) {
            GameObject EnemyCopy = Instantiate (enemyTypes[0]);
            EnemyCopy.transform.rotation = item.transform.rotation;
            EnemyCopy.transform.position = item.transform.position;
            EnemyCopy.SetActive (true);
        }

    }

    // Update is called once per frame
    void Update () {

    }
}