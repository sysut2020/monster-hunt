using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy Behavior test move down
/// test behavior that moves the enemy downwards
/// </summary>

[RequireComponent (typeof (Enemy))]
public class EBTestMoveDown : MonoBehaviour, IEnemyBehavior {

    private Enemy enemy;
    private float velocity;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start () {
        this.enemy = this.gameObject.GetComponent<Enemy> ();
        this.velocity = enemy.EnemyType.speed;
    }
    public void Act () {
        transform.Translate (new Vector2 (0f, velocity));
    }

    public void OnDead () {
        Destroy (this.gameObject);
        string l = SudoRandomLetterGenerator.Instance.GenerateLetter ();
        print (l);
    }

}