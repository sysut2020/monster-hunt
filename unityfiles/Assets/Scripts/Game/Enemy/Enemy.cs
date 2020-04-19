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
[RequireComponent (typeof (EnemyHealthController))]
public class Enemy : MonoBehaviour, IKillable, IDamageNotifyable {

    [Tooltip ("A sciptable object representing the enemy type")]
    [SerializeField]
    private EnemyType enemyType;

    private bool isAttacking = false;
    private EnemyHealthController enemyHealthController;

    public bool IsAttacking {
        get => this.isAttacking;
        set => this.isAttacking = value;
    }

    public EnemyType EnemyType {
        get => this.enemyType;
        set => this.enemyType = value;
    }

    /// <summary>
    /// Fire enemy killed event and destroy self.
    /// </summary>
    public void IsDead () {
        EnemyEventArgs args = new EnemyEventArgs {
            EnemyType = this.enemyType,
            Position = this.transform.position
        };
        EnemyKilledEvent?.Invoke (this, args);
        Destroy (this.gameObject);
    }

    public static event EventHandler<EnemyEventArgs> EnemySpawnEvent;
    public static event EventHandler<EnemyEventArgs> EnemyKilledEvent;

    private void Awake () {
        this.enemyHealthController = this.gameObject.GetComponent<EnemyHealthController> ();
        this.enemyHealthController.StartHealth = this.EnemyType.Health;
    }

    void Start () {
        this.tag = "Enemy";
        EnemyEventArgs args = new EnemyEventArgs ();
        args.Position = this.gameObject.transform.position;
        args.EnemyType = this.enemyType;
        EnemySpawnEvent?.Invoke (this, args);
    }

    /// <summary>
    /// Notify this component that it has taken damge.
    /// </summary>
    public void Damaged () {
        // Trigger damage anumation
    }
}