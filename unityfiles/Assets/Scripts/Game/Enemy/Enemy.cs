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

    // -- public -- //

    public void Dead() {
        // act dead
    }
    // -- private -- //

    // -- unity -- // 
    void Start() {
        this.tag = "Enemy";
        this.healthController = this.gameObject.GetComponent<HealthController>();
    }

}