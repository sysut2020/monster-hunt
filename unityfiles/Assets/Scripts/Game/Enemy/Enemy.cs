using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describes an enemy
/// </summary>
public class Enemy : MonoBehaviour, IDamageable {

    [Tooltip("A sciptable object representing the enemy type")]
    [SerializeField]
    private EnemyType enemyType;

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

    private void Update() {
        if (Input.GetKeyDown(KeyCode.K)) {
            Dead();
        }
    }

    // -- public -- //

    public void Dead() {
        // act dead
        Destroy(this);
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