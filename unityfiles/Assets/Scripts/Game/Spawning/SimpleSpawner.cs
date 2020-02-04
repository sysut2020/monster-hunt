using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// THIS CLASS IS FOR TESTING YOU SHOULD NOT SEE THIS
public class SimpleSpawner : MonoBehaviour {
    // -- inspector -- //
    [SerializeField]
    private float spawnInterval;

    [SerializeField]
    private Texture2D enemyTexture;

    [Tooltip ("the enemy type.")]
    [SerializeField]
    private EnemyType enemyTypeToSpawn;

    // -- internal -- //
    private float lastSpawn;
    private GameObject enemyBlueprint;

    // -- private -- //
    private void GenerateBlueprint () {
        GameObject ebp = new GameObject ();
        this.enemyBlueprint = ebp;
        ebp.name = "Enemy";
        ebp.tag = "Enemy";

        SpriteRenderer spriteRender = ebp.AddComponent<SpriteRenderer> ();
        spriteRender.sprite = Sprite.Create (this.enemyTexture, new Rect (0, 0, this.enemyTexture.width, this.enemyTexture.height), Vector2.zero);
        ebp.transform.localScale = new Vector3 (0.025f, 0.025f, 1);

        BoxCollider2D bc = ebp.AddComponent<BoxCollider2D> ();

        Enemy enemyScript = ebp.AddComponent<Enemy> () as Enemy;

        enemyScript.EnemyType = this.enemyTypeToSpawn;
      

        IEnemyBehavior EBH = ebp.AddComponent<EBTestMoveDown> ();

        HealthController healthController = ebp.AddComponent<HealthController> ();
        healthController.EntityHealth = 5f;

        ebp.SetActive (false);
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