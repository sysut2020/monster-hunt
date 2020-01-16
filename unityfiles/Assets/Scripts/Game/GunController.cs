using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Til trygve husk og fjern

Do use camelCasing for member variables
Do use camelCasing for parameters
Do use camelCasing for local variables
Do use PascalCasing for function, property, event, and class names
Do prefix interfaces names with “I”
Do not prefix enums, classes, or delegates with any letter 

*/
public class GunController : MonoBehaviour
{

    // -- inspector

    [Header("Bullet properties")]

    [Tooltip("velocety of bullet in units/(1/50) sec.")]
    [SerializeField]
    private Vector2 bulletVelocity;

    [Tooltip("Damage inflicted by bullet.")]
    [SerializeField] 
    private float bulletDamage;

    [Tooltip("How long the bullet wil live if it dosent hit anything.")]
    [SerializeField] 
    private float bulletTtl;

    [Tooltip("the 2d texture tu use on the bullet sprite.")]
    [SerializeField] 
    private Texture2D bulletTexure;

    [Tooltip("the scale of the sprite used.")]
    [SerializeField] 
    private Vector2 spriteScale;

    [Header("Fireing properties")]

    [Tooltip("Fire rate in shots/sec.")]
    [SerializeField] 
    private float fireRate;   


    // -- internal
    private ArrayList activeBullets = new ArrayList();
    private ArrayList idleBullets = new ArrayList();
    private bool isFirering;
    private float nextBRT;      // bullet relase time
    private float bulletDeltaT;

    private GameObject bluprintBullet;


    // -- public
     public void StartFirering()
    {
        this.isFirering = true;
    }

    public void StopFirering()
    {
        this.isFirering = false;
    }

    // generates a new bullet and fires it
    void FireNewProjetile(){
        GameObject bulletCopy = Instantiate(this.bluprintBullet);
        bulletCopy.transform.rotation = this.transform.rotation;
        bulletCopy.transform.position = this.transform.position;
        bulletCopy.SetActive(true);

        this.activeBullets.Add(bulletCopy);

        this.nextBRT = Time.time + this.bulletDeltaT;
    }

    // reuses one of the inactive bullets and fires it
    void FireExistingProjectile(){
        GameObject bulletCopy = (GameObject)this.idleBullets[0];
        this.idleBullets.Remove(bulletCopy);

        bulletCopy.transform.rotation = this.transform.rotation;
        bulletCopy.transform.position = this.transform.position;
        bulletCopy.SetActive(true);

        this.activeBullets.Add(bulletCopy);

        this.nextBRT = Time.time + this.bulletDeltaT;
    }

    // -- Mono b

    // Start is called before the first frame update
    void Start()
    {
        this.nextBRT = Time.time;
        this.bulletDeltaT = 1 / this.fireRate;

        // blue print bullet generation
        GameObject bullet = new GameObject();
        this.bluprintBullet = bullet;
        bullet.name = "Bullet";

        SpriteRenderer spriteRend = bullet.AddComponent<SpriteRenderer>() as SpriteRenderer;
        spriteRend.sprite = Sprite.Create(this.bulletTexure, new Rect(0,0,this.bulletTexure.width ,this.bulletTexure.height), Vector2.zero);
        bullet.transform.localScale = this.spriteScale;

        BoxCollider2D boxCol = bullet.AddComponent<BoxCollider2D>() as BoxCollider2D;

        BulletControll bulletControll = bullet.AddComponent<BulletControll>() as BulletControll;

        //print(bulletControll.Velocety);
        bulletControll.Velocety = this.bulletVelocity;
        //print(this.bulletVelocity);
        //print(bulletControll.Velocety);
        bulletControll.Damage = this.bulletDamage;
        bulletControll.Ttl = 20.0f;



        Rigidbody2D rigidB2d = bullet.AddComponent<Rigidbody2D>() as Rigidbody2D;
        rigidB2d.bodyType = RigidbodyType2D.Kinematic;


        print(bulletControll.Velocety);
        bullet.SetActive(false);
        print(bulletControll.Velocety);
        

        //        Texture2D sTexture = (Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Resources/Graphics/banana_PNG835.png", typeof(Texture2D));
        //spriteRend.sprite = Sprite.Create(sTexture, new Rect(0,0,100 ,100), Vector2.zero);


    }
    

    // Update is called once per frame
    void Update()
    {
        // is it time to release a bullet
        if (Time.time >= this.nextBRT){
            // if yes is ther any bullets in the active bullet list that is inactive
            for (int i = this.activeBullets.Count; i > 0 ; i--)
            {
                GameObject g = (GameObject)this.activeBullets[i-1];
                //det her kan umulig ver veldig effektivt
                if (!g.activeInHierarchy){
                    //if there is move them to the inactive list
                    this.activeBullets.Remove(g);
                    this.idleBullets.Add(g);
                }
            }
            // if there are any inactive bullets fire them if not make a new one
            if (this.idleBullets.Count >= 1)
            {
                this.FireExistingProjectile();
            } else
            {
                this.FireNewProjetile();
            }
        }
    }

    
   
}
