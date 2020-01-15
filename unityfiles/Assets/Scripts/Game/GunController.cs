using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    // shots/sec 
    public float fireRate;

    public Vector2 bulletVelocity;

    public float bulletDamage;


    // fire controll stuff
    private ArrayList activeBullets = new ArrayList();
    private ArrayList idleBullets = new ArrayList();
    private bool isFirering;
    private float nextBRT; // bullet relase time
    private float bulletDeltaT;

    private GameObject BluprintBullet;




    // Start is called before the first frame update
    void Start()
    {
        this.nextBRT = Time.time;
        this.bulletDeltaT = 1 / this.fireRate;

        // blue print bullet generation
        GameObject bpb = new GameObject();
        this.BluprintBullet = bpb;
        bpb.name = "Bullet";



        SpriteRenderer spriteRend = bpb.AddComponent<SpriteRenderer>() as SpriteRenderer;
        Texture2D sTexture = (Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Resources/Graphics/banana_PNG835.png", typeof(Texture2D));
        spriteRend.sprite = Sprite.Create(sTexture, new Rect(0,0,sTexture.width ,sTexture.height), Vector2.zero);
        //spriteRend.sprite = Sprite.Create(sTexture, new Rect(0,0,100 ,100), Vector2.zero);
        bpb.transform.localScale = new Vector3(0.025f,0.025f,1);

        BoxCollider2D bc = bpb.AddComponent<BoxCollider2D>() as BoxCollider2D;

        BulletControll BcScript = bpb.AddComponent<BulletControll>() as BulletControll;
        BcScript.velocety = this.bulletVelocity;
        BcScript.damage = this.bulletDamage;
        BcScript.ttl = 20;

        Rigidbody2D Rgb2D = bpb.AddComponent<Rigidbody2D>() as Rigidbody2D;
        Rgb2D.bodyType = RigidbodyType2D.Kinematic;

        bpb.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= this.nextBRT){
            for (int i = this.activeBullets.Count; i > 0 ; i--)
            {
                GameObject g = (GameObject)this.activeBullets[i-1];
                //det her kan umulig ver veldig effektivt
                if (!g.activeInHierarchy){
                    this.activeBullets.Remove(g);
                    this.idleBullets.Add(g);
                }
            }
            
            if (this.idleBullets.Count >= 1){
                GameObject bulletCopy = (GameObject)this.idleBullets[0];
                this.idleBullets.Remove(bulletCopy);

                bulletCopy.transform.rotation = this.transform.rotation;
                bulletCopy.transform.position = this.transform.position;
                bulletCopy.SetActive(true);

                this.activeBullets.Add(bulletCopy);

                this.nextBRT = Time.time + this.bulletDeltaT;

            } else{
                GameObject bulletCopy = Instantiate(this.BluprintBullet);
                bulletCopy.transform.rotation = this.transform.rotation;
                bulletCopy.transform.position = this.transform.position;
                bulletCopy.SetActive(true);

                this.activeBullets.Add(bulletCopy);

                this.nextBRT = Time.time + this.bulletDeltaT;
            }
        }
    }

    void fireProjetile(){
        

    }

    public void startFirering(){
        this.isFirering = true;
    }

    public void stopFirering(){
        this.isFirering = false;
    }
}
