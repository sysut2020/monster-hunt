using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Loads a new scene by its scene name
/// Throws ArgumentNullException if the name is NULL
/// </summary>
/// <param name="sceneName">the name of the scene</param>

/// <summary>
/// The scene manager is responsible for loading scenes.
/// The scene manager is a singleton, and there can only exist a single
/// instane of it.
/// It can load a scene by name and its build index.
/// </summary>
/// <returns> A list of the Game objects children  </returns>

// -- properties -- //
// -- public -- //
// -- private -- // 


/// <summary>
/// Responsible for Spawning enemys.
/// 
/// </summary>
public class Spawner : MonoBehaviour
{

    // -- editor
    [Header("spawning methods")]
    [Tooltip("spawn on and one")]
    [SerializeField]
    private bool spawnSingle = false;

    [Tooltip("chek if theere is space to spawn if not dont")]
    [SerializeField]
    private bool chekIfSpace = true;

    [Tooltip("Time betwen spawns")]
    [SerializeField]
    private float timeBetweenSpawns= 0;


    [Header("Enemy Types")]
    [Tooltip("the types of enemy to spawn")]
    [SerializeField]
    private List<GameObject> enemyTypes = new List<GameObject>();



    // -- internal 

    private List<GameObject> spawnPoints = new List<GameObject>();

    /// <summary>
    /// Get's a list off all the children of the current game object
    /// </summary>
    /// <returns> A list of the Game objects children  </returns>
    List<GameObject> getGOChildren()
    {
        List<GameObject> children = new List<GameObject>();

        int numSpawnpoints = this.gameObject.transform.childCount;

        for (int i = 0; i < numSpawnpoints; i++)
        {
            children.Add(this.gameObject.transform.GetChild(i).gameObject);
        }   
        

        return children;
    }





    // -- unity
    void Start()
    {

        foreach (GameObject item in spawnPoints)
        {
            GameObject EnemyCopy = Instantiate(enemyTypes[0]);
            EnemyCopy.transform.rotation = item.transform.rotation;
            EnemyCopy.transform.position = item.transform.position;
            EnemyCopy.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
