using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntetyHealth : MonoBehaviour
{
    public float Health;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.Health <= 0f)
        {
            Debug.Log(this.Health);
            Debug.Log("Entity killed");
            Destroy(this.gameObject);
        }
    }

    public void ApplyDamage(float dmg){
        // TODO: mulg debonce her 
        this.Health = this.Health - dmg;
        //Debug.Log(this.Health);
    }


    
}
