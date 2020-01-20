using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BulletControll : MonoBehaviour
{

    // -- internal
    [SerializeField] 
    private Vector2 velocity;
    [SerializeField]
    private float bulletTimeToLive;                      // Time to live
    [SerializeField]
    private float damage;

    // -- properties


    public Vector2 Velocety 
    {   
        get{return this.velocity;} 
        set{this.velocity = value;}
    } 
    public float BulletTimeToLive 
    {   
        get{return this.bulletTimeToLive;} 
        set{this.bulletTimeToLive = value;}
    }

    public float Damage
    {   
        get{return this.damage;} 
        set{this.damage = value;}
    }


    private float killAt;
    private bool isKillable;


    // Starts the time to live timer
    public void StartTtlTimer(){
        this.killAt = Time.time + this.bulletTimeToLive;
    }


    // disables the bullet
    void KillSelf(){
        this.gameObject.SetActive(false);
    }

    // -- Mono b

    void Start()
    {
        this.StartTtlTimer();   // to avoid potensial instant removal
        this.tag = "Bullet";
    }

    void FixedUpdate() // 50 cals per sec
    {
        this.gameObject.transform.Translate(this.velocity.x, this.velocity.y, 0);


        if (Time.time > this.killAt & this.isKillable){
            this.isKillable = false;
            this.KillSelf();
        }
    }

    void OnTriggerEnter2D(Collider2D Col) 
    {
    if (Col.tag == "Enemy"){
        Debug.Log("hit");
        
        HealthController enemy = (HealthController)Col.gameObject.GetComponent("HealthController");
        enemy.ApplyDamage(this.damage);

        this.KillSelf();
        }
    }

    void OnEnable(){
        this.isKillable = true;
        this.StartTtlTimer();
    }


    
}
