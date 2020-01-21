using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SimpleSpawner : MonoBehaviour
{
    // -- inspector
    [SerializeField]
    private float spawnInterval;

    [SerializeField]
    private Texture2D enemyTexture;

    // -- internal
    private float lastSpawn;
    private GameObject enemyBlueprint;
    // Start is called before the first frame update
    void Start()
    {
        // TODO FLYTT TIL SPIL INIT SCRIPT
        SudoRandomLetterGenerator tmp = SudoRandomLetterGenerator.Instance;


        this.lastSpawn = Time.time;
        GameObject ebp = new GameObject();
        this.enemyBlueprint = ebp;
        ebp.name = "Enemy";
        ebp.tag = "Enemy";


        SpriteRenderer spriteRender = ebp.AddComponent<SpriteRenderer>();
        spriteRender.sprite = Sprite.Create(this.enemyTexture, new Rect(0,0,this.enemyTexture.width ,this.enemyTexture.height), Vector2.zero);
        ebp.transform.localScale = new Vector3(0.25f,0.25f,1);

        BoxCollider2D bc = ebp.AddComponent<BoxCollider2D>();

        Enemy enemyScript = ebp.AddComponent<Enemy>() as Enemy;

        
        HealthController healthController = ebp.AddComponent<HealthController>();
        healthController.EntityHealth = 5f;


        ebp.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > this.lastSpawn + this.spawnInterval){
            GameObject EnemyCopy = Instantiate(this.enemyBlueprint);
            EnemyCopy.transform.rotation = this.transform.rotation;
            EnemyCopy.transform.position = this.transform.position;
            EnemyCopy.SetActive(true);

            this.lastSpawn = Time.time;
        }
        
    }
}
