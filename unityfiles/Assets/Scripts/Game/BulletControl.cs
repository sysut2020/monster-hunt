using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
TODO: maybe blop this in an interface like the HC/enemy relation
*/

public class BulletControl : MonoBehaviour
{

    // -- internal -- // 
    [SerializeField] 
    private Vector2 velocity;
    [SerializeField]
    private float bulletTimeToLive;
    [SerializeField]
    private float damage;

    // -- properties -- //

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
    private bool isActive;


    /// <summary>
    /// Starts the time to live timer
    /// 
    /// </summary>
    public void StartTtlTimer(){
        this.killAt = Time.time + this.bulletTimeToLive;
    }


    /// <summary>
    /// Disables the game object so it can be 
    /// collected in the local gun controller
    /// </summary>
    void KillSelf(){
        this.gameObject.SetActive(false);
    }

    // -- unity -- //

    void Start()
    {
        this.StartTtlTimer();   // to avoid potensial instant removal
        this.tag = "Bullet";
    }

    void FixedUpdate() // 50 calls per sec
    {
        this.gameObject.transform.Translate(this.velocity.x, this.velocity.y, 0);

        if (Time.time > this.killAt && this.isActive){
            this.isActive = false;
            this.KillSelf();
        }
    }

    void OnTriggerEnter2D(Collider2D Col) 
    {
        if (Col.tag == "Enemy"){
            
            HealthController enemy = (HealthController)Col.gameObject.GetComponent("HealthController");
            enemy.ApplyDamage(this.damage);

            this.KillSelf();
        }
    }

    void OnEnable(){
        this.isActive = true;
        this.StartTtlTimer();
    }


    
}
