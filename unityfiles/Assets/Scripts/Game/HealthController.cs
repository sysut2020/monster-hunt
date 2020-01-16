using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    //[Tooltip("the ammont of heth a given entety has.")]
    [SerializeField] 
    private float entityHealth;

    // -- properties

    public float EntityHealth
    {   
        get{return this.entityHealth;} 
        set{this.entityHealth = value;}
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.entityHealth <= 0f)
        {
            Debug.Log("Entity killed");
            Destroy(this.gameObject);
        }
    }

    public void ApplyDamage(float dmg){
        // TODO: mulg debonce her 
        this.entityHealth = this.entityHealth - dmg;
        Debug.Log(this.entityHealth);
    }


    
}
