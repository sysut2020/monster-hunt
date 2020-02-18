using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describes an enemy
/// </summary>
[RequireComponent(typeof(HealthController))]
public class Enemy : MonoBehaviour, IDamageable {

    [Tooltip("A sciptable object representing the enemy type")]
    [SerializeField]
    private EnemyType enemyType;

    private bool isAttacking = false;

    public bool IsAttacking {
        get { return this.isAttacking; }
        set { this.isAttacking = value; }
    }

    private HealthController healthController;

    [Tooltip("The front point a raycast is sent from")]
    [SerializeField]
    private Transform frontPoint;

    public EnemyType EnemyType {
        get { return this.enemyType; }
        set { this.enemyType = value; }
    }

    public Transform FrontPoint {
        get { return this.frontPoint; }
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

    private void OnDestroy() {
        OnEnemyDead?.Invoke();
    }

    public static event Action OnEnemyDead;
}