using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describes an enemy
/// </summary>
public class Enemy : MonoBehaviour, IDamageable {

    [Tooltip("the enemy type.")]
    [SerializeField]
    private EnemyType enemyType;

    private HealthController healthController;
    private IEnemyBehavior enemyBehavior;

    [Tooltip("The front point a raycast is sent from")]
    [SerializeField]
    private Transform frontPoint;

    [Tooltip("The rear point a raycast is sent from")]
    [SerializeField]
    private Transform rearPoint;

    public EnemyType EnemyType {
        get { return this.enemyType; }
        set { this.enemyType = value; }
    }

    public Transform FrontPoint {
        get { return this.frontPoint; }
    }

    public Transform RearPoint {
        get { return this.rearPoint; }
    }

    // -- public -- //

    public void Dead() {
        this.enemyBehavior.OnDead();
    }
    // -- private -- //

    // -- unity -- // 
    void Start() {
        this.tag = "Enemy";

        this.healthController = this.gameObject.GetComponent<HealthController>();

        this.enemyBehavior = this.gameObject.GetComponent<IEnemyBehavior>();

        BoxCollider2D bc;
        bc = gameObject.AddComponent<BoxCollider2D>() as BoxCollider2D;
        bc.isTrigger = true;

    }

    void FixedUpdate() {
        //this.enemyBehavior.Act();

    }

}