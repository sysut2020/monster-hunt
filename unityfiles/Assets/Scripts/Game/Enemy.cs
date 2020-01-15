using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        this.tag = "Enemy";

        BoxCollider2D bc;
        bc = gameObject.AddComponent<BoxCollider2D>() as BoxCollider2D;
        bc.isTrigger = true;
   
    }

    void FixedUpdate(){
        transform.Translate(new Vector2(0f,-0.05f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
