using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEventArgs : EventArgs {
    public Vector3 Position { get; set; }
    public EnemyType EnemyType { get; set; }
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
        get => this.frontPoint;
    }

    // -- public -- //

    /// <summary>
    /// Handles what to do when the enemy is killed
    /// </summary>
    public void Dead() {
        EnemyEventArgs args = new EnemyEventArgs();
        args.EnemyType = this.enemyType;
        args.Position = this.transform.position;
        EnemyKilledEvent?.Invoke(this, args);
        Destroy(this.gameObject);
    }
    // -- events -- //
    public static event EventHandler<EnemyEventArgs> EnemySpawnEvent;
    public static event EventHandler<EnemyEventArgs> EnemyKilledEvent;

    // -- private -- //

    // -- unity -- // 
    void Start() {
        this.tag = "Enemy";
        this.healthController = this.gameObject.GetComponent<HealthController>();
        this.healthController.EntityHealth = this.EnemyType.Health;
        EnemyEventArgs args = new EnemyEventArgs();
        args.Position = this.gameObject.transform.position;
        args.EnemyType = this.enemyType;
        EnemySpawnEvent?.Invoke(this, args);
    }
    
}