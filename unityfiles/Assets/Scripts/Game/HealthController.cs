using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [Tooltip("the ammont of heth a given entety has.")]
    [SerializeField] 
    private float entityHealth;

    private bool isDead = false;

    public bool IsDead
    {   
        get{return this.isDead;} 
        internal set{this.isDead = value;}
    }

    // -- properties

    public float EntityHealth
    {   
        get{return this.entityHealth;} 
        set{this.entityHealth = value;}
    }

    // -- public
    public void ApplyDamage(float dmg){
        // TODO: mulg debonce her 
        this.entityHealth = this.entityHealth - dmg;
        //Debug.Log(this.entityHealth);
    }


    

    // -- unity
    void Update()
    {
        if (this.entityHealth <= 0f)
        {
            //Debug.Log("Entity killed");
            //print(SudoRandomLetterGenerator.Instance.GenerateLetter());// placeholder til grafikken e på plass
            string l = SudoRandomLetterGenerator.Instance.GenerateLetter();
            Destroy(this.gameObject);
        }
    }

    

    
}
