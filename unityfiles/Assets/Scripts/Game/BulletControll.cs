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


public class BulletControll : MonoBehaviour
{

    // -- internal
    [SerializeField] 
    private Vector2 velocety;
    [SerializeField]
    private float ttl;                      // Time to live
    [SerializeField]
    private float damage;

    // -- properties


    public Vector2 Velocety 
    {   
        get{return this.velocety;} 
        set{this.velocety = value;}
    } 
    public float Ttl 
    {   
        get{return this.ttl;} 
        set{this.ttl = value;}
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
        this.killAt = Time.time + this.ttl;
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
        this.gameObject.transform.Translate(this.velocety.x, this.velocety.y, 0);


        if (Time.time > this.killAt & this.isKillable){
            this.isKillable = false;
            this.KillSelf();
        }
    }

    void OnTriggerEnter2D(Collider2D Col) {
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
