using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControll : MonoBehaviour
{
    //public bool active;

    // units/tick  50/s
    public Vector2 velocety;
    public float ttl;
    public float damage;

    private float killAt;

    // Start is called before the first frame update
    void Start()
    {
        //this.active = true;
        this.ttl = 5f;
        this.damage = 100;

        this.killAt = Time.time + ttl;
        this.velocety = new Vector2(0.05f,0f);
        this.tag = "Bullet";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(velocety);

        //if (Time.time > killAt){
        //    this.KillSelf();    
        //}
    }

    void OnTriggerEnter2D(Collider2D Col) {
    if (Col.tag == "Enemy"){
        Debug.Log("hit");
        
        EntetyHealth enemy = (EntetyHealth)Col.gameObject.GetComponent("EntetyHealth");
        enemy.ApplyDamage(this.damage);

        this.KillSelf();
        }
    }


    void KillSelf(){
        this.gameObject.SetActive(false);
        //this.active = false;
        //Destroy(this.gameObject);
    }
}
