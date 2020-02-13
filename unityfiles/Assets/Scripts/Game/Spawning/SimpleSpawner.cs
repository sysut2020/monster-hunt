using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// THIS CLASS IS FOR TESTING YOU SHOULD NOT SEE THIS
public class SimpleSpawner : MonoBehaviour {
    // -- inspector -- //
    [SerializeField]
    private float spawnInterval = 1;

    [SerializeField]
    private Texture2D enemyTexture = null;

    [Tooltip ("the enemy type.")]
    [SerializeField]
    private EnemyType enemyTypeToSpawn = null;

    private float lastSpawn = 0;
    private GameObject enemyBlueprint = null;

    // -- singelton -- //

    
    private static SimpleSpawner instance;

    public static SimpleSpawner Instance{
        get {
            if (instance == null) {
                instance = new SimpleSpawner();
            }

            return instance;
        }
    }

    private void Awake(){
        instance = (SimpleSpawner.Instance != null)? new SimpleSpawner(): SimpleSpawner.Instance;
    }

    // -- private -- //
    private void GenerateBlueprint () {
        this.enemyBlueprint = new GameObject ();
        this.enemyBlueprint.name = "Enemy";
        this.enemyBlueprint.tag = "Enemy";

        SpriteRenderer spriteRender = this.enemyBlueprint.AddComponent<SpriteRenderer> ();
        spriteRender.sprite = Sprite.Create (this.enemyTexture, new Rect (0, 0, this.enemyTexture.width, this.enemyTexture.height), Vector2.zero);
        this.enemyBlueprint.transform.localScale = new Vector3 (0.025f, 0.025f, 1);

        BoxCollider2D bc = this.enemyBlueprint.AddComponent<BoxCollider2D> ();

        Enemy enemyScript = this.enemyBlueprint.AddComponent<Enemy> () as Enemy;

        enemyScript.EnemyType = this.enemyTypeToSpawn;
      

        //IEnemyBehavior EBH = this.enemyBlueprint.AddComponent<EBTestMoveDown> ();

        HealthController healthController = this.enemyBlueprint.AddComponent<HealthController> ();
        healthController.EntityHealth = 5f;

        this.enemyBlueprint.SetActive (false);
    }

    // -- unity -- //
    void Start () {
        // TODO MOVE TO INIT SCRIPT
        SudoRandomLetterGenerator tmp = SudoRandomLetterGenerator.Instance;

        this.lastSpawn = Time.time;

        this.GenerateBlueprint ();

    }

    void Update () {
        if (Time.time > this.lastSpawn + this.spawnInterval) {
            GameObject EnemyCopy = Instantiate (this.enemyBlueprint);
            EnemyCopy.transform.rotation = this.transform.rotation;
            EnemyCopy.transform.position = this.transform.position;
            EnemyCopy.SetActive (true);

            this.lastSpawn = Time.time;
        }

    }
}