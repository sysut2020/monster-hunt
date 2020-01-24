using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for Spawning enemys.
/// 
/// </summary>
public class Enemy : MonoBehaviour, IDamageble
{
  
    [Tooltip("the enemy type.")]
    [SerializeField] 
    private EnemyType enemyType;




    private HealthController healthController;
    private IEnemyBehavior enemyBehavior;
    // -- properties -- //

    public EnemyType EnemyType
    {   
        get{return this.enemyType;} 
        set{this.enemyType = value;}
    }


    // -- public -- //

    public void dead(){
        this.enemyBehavior.OnDead();
    }
    // -- private -- //




    // -- unity -- // 
    void Start()
    {
        this.tag = "Enemy";

        this.healthController = this.gameObject.GetComponent<HealthController>();


        this.enemyBehavior = this.gameObject.GetComponent<IEnemyBehavior>();

        BoxCollider2D bc;
        bc = gameObject.AddComponent<BoxCollider2D>() as BoxCollider2D;
        bc.isTrigger = true;

        
    }


    void FixedUpdate(){
        this.enemyBehavior.Act();
        
    }

    
}
