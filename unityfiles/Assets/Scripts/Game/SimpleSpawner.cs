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
    private GameObject enemyBluprint;
    // Start is called before the first frame update
    void Start()
    {
        this.lastSpawn = Time.time;
        GameObject ebp = new GameObject();
        this.enemyBluprint = ebp;
        ebp.name = "Enemy";
        ebp.tag = "Enemy";



        SpriteRenderer spriteRend = ebp.AddComponent<SpriteRenderer>();
        //Texture2D sTexture = (Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Resources/Graphics/international-space-station.png", typeof(Texture2D));
        spriteRend.sprite = Sprite.Create(this.enemyTexture, new Rect(0,0,this.enemyTexture.width ,this.enemyTexture.height), Vector2.zero);
        //spriteRend.sprite = Sprite.Create(sTexture, new Rect(0,0,100 ,100), Vector2.zero);
        ebp.transform.localScale = new Vector3(0.25f,0.25f,1);

        BoxCollider2D bc = ebp.AddComponent<BoxCollider2D>();

        Enemy EnScript = ebp.AddComponent<Enemy>() as Enemy;

        
        HealthController healthController = ebp.AddComponent<HealthController>();
        healthController.EntityHealth = 5f;

        ebp.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > this.lastSpawn + this.spawnInterval){
            GameObject EnemyCopy = Instantiate(this.enemyBluprint);
            EnemyCopy.transform.rotation = this.transform.rotation;
            EnemyCopy.transform.position = this.transform.position;
            EnemyCopy.SetActive(true);

            this.lastSpawn = Time.time;
        }
        
    }
}
