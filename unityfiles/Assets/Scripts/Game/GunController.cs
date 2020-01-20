using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GunController : MonoBehaviour
{

    // -- inspector

    [Header("Bullet properties")]

    [Tooltip("velocity of bullet in units/(1/50) sec.")]
    [SerializeField]
    private Vector2 bulletVelocity;

    [Tooltip("Damage inflicted by bullet.")]
    [SerializeField] 
    private float bulletDamage;

    [Tooltip("How long the bullet will live if it doesn´t hit anything.")]
    [SerializeField] 
    private float bulletTtl;

    [Tooltip("2d texture for bullet sprite.")]
    [SerializeField] 
    private Texture2D bulletTexture;

    [Tooltip("the scale of the sprite used.")]
    [SerializeField] 
    private Vector2 spriteScale;

    [Header("Firing properties")]

    [Tooltip("Fire rate in shots/sec.")]
    [SerializeField] 
    private float fireRate;   


    // -- internal
    private ArrayList activeBullets = new ArrayList();
    private ArrayList idleBullets = new ArrayList();
    private bool isFiring;
    private float nextBulletReleaseTime;      // bullet release time
    private float bulletDeltaTime;

    private GameObject blueprintBullet;


    // -- public
     public void startFiring()
    {
        this.isFiring = true;
    }

    public void StopFiring()
    {
        this.isFiring = false;
    }

    // generates a new bullet and fires it
    void FireNewProjectile(){
        GameObject bulletCopy = Instantiate(this.blueprintBullet);
        bulletCopy.transform.rotation = this.transform.rotation;
        bulletCopy.transform.position = this.transform.position;
        bulletCopy.SetActive(true);

        this.activeBullets.Add(bulletCopy);

        this.nextBulletReleaseTime = Time.time + this.bulletDeltaTime;
    }

    // reuses one of the inactive bullets and fires it
    void FireExistingProjectile(){
        GameObject bulletCopy = (GameObject)this.idleBullets[0];
        this.idleBullets.Remove(bulletCopy);

        bulletCopy.transform.rotation = this.transform.rotation;
        bulletCopy.transform.position = this.transform.position;
        bulletCopy.SetActive(true);

        this.activeBullets.Add(bulletCopy);

        this.nextBulletReleaseTime = Time.time + this.bulletDeltaTime;
    }

    // -- Mono b

    // Start is called before the first frame update
    void Start()
    {
        this.nextBulletReleaseTime = Time.time;
        this.bulletDeltaTime = 1 / this.fireRate;

        // blue print bullet generation
        GameObject bullet = new GameObject();
        this.blueprintBullet = bullet;
        bullet.name = "Bullet";

        SpriteRenderer spriteRender = bullet.AddComponent<SpriteRenderer>() as SpriteRenderer;
        spriteRender.sprite = Sprite.Create(this.bulletTexture, new Rect(0,0,this.bulletTexture.width ,this.bulletTexture.height), Vector2.zero);
        bullet.transform.localScale = this.spriteScale;

        BoxCollider2D boxCol = bullet.AddComponent<BoxCollider2D>() as BoxCollider2D;

        BulletControl bulletControl = bullet.AddComponent<BulletControl>() as BulletControl;

        bulletControl.Velocety = this.bulletVelocity;
        bulletControl.Damage = this.bulletDamage;
        bulletControl.BulletTimeToLive = this.bulletTtl;



        Rigidbody2D rigidB2d = bullet.AddComponent<Rigidbody2D>() as Rigidbody2D;
        rigidB2d.bodyType = RigidbodyType2D.Kinematic;


        bullet.SetActive(false);
        

        //        Texture2D sTexture = (Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Resources/Graphics/banana_PNG835.png", typeof(Texture2D));
        //spriteRend.sprite = Sprite.Create(sTexture, new Rect(0,0,100 ,100), Vector2.zero);


    }
    

    // Update is called once per frame
    void Update()
    {
        // is it time to release a bullet
        if (Time.time >= this.nextBulletReleaseTime){
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
                this.FireNewProjectile();
            }
        }
    }

    
   
}
