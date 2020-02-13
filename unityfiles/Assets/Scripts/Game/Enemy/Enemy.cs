using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyEventArgs: EventArgs{
    public GameObject EnemyGO {get; set;}
    public EnemyType EnemyType {get; set;}
}

public static class EnemyEvents{
    public static event EventHandler<EnemyEventArgs> EnemySpawnEvent;
    public static event EventHandler<EnemyEventArgs> EnemyKilledEvent;
}

/// <summary>
/// Describes an enemy
/// </summary>
[RequireComponent(typeof(HealthController))]
public class Enemy : MonoBehaviour, IDamageable {

    [Tooltip("A sciptable object representing the enemy type")]
    [SerializeField]
    private EnemyType enemyType;

    [Tooltip("The front point a raycast is sent from")]
    [SerializeField]
    private Transform frontPoint;

    private bool isAttacking = false;
    private HealthController healthController;

    

    // -- properties -- //

    public bool IsAttacking {
        get => this.isAttacking;
        set => this.isAttacking = value;
    }

    public EnemyType EnemyType {
        get => this.enemyType; 
        set => this.enemyType = value; 
    }

    public Transform FrontPoint {
        get =>  this.frontPoint; 
    }

    // -- public -- //

    public void Dead() {
        // act dead
        GameObject.Destroy(gameObject);
        CollectibleSpawner.Instance.SpawnCollectible(this.transform.position);
    }
    // -- private -- //

    // -- unity -- // 
    void Start() {
        this.tag = "Enemy";
        this.healthController = this.gameObject.GetComponent<HealthController>();
    }

}