using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEventArgs : EventArgs {
    public GameObject PlayerGO { get; set; }
    public EnemyType PlayerMoney { get; set; }
    public EnemyType PlayerLetters { get; set; }
}

/// <summary>
/// this class describes the player
/// </summary>
public class Player : MonoBehaviour, IDamageable {

    [SerializeField]
    private PlayerHealthController playerHealthController;

    

    private Animator animator;

    public delegate void EventHandler();

    public static event EventHandler OnPlayerDead;

    private PlayerWeaponController playerWeaponController;

    private Vector3 spawnPosition;

    // -- singelton -- //

    private static Player instance;

    public static Player Instance {
        get {
            if (instance == null) {
                GameObject player = new GameObject("Player");
                player.AddComponent<Player>();
            }

            return instance;
        }
    }

    // -- properties -- //

    public PlayerHealthController PlayerHealthController {
        get => playerHealthController;
        internal set => this.playerHealthController = value;
    }

    public PlayerWeaponController PlayerWeaponController {
        get => playerWeaponController;
        internal set => this.playerWeaponController = value;
    }

    // -- public -- //


    /// <summary>
    /// Respawns the player and notify that the player died
    /// </summary>
    public void Dead() {
        Respawn();
        PlayerEventArgs args = new PlayerEventArgs();
        PlayerKilledEvent?.Invoke(this, args);
    }
    // -- events -- //

    public static event EventHandler<PlayerEventArgs> PlayerKilledEvent;
    private void SubscribeToEvents() { 
        PlayerWeaponController.WeaponChangedEvent += CallbackWeaponChangedEvent;
    }

    private void UnsubscribeFromEvents() {
        PlayerWeaponController.WeaponChangedEvent -= CallbackWeaponChangedEvent;
;
    }


    private void CallbackWeaponChangedEvent(object _, WeaponChangedEventArgs args){
        animator.SetInteger("ACTIVE_WEAPON", (int)args.AnimId);
    }

    // -- private -- // 

    /// <summary>
    /// Moves the player to the spawn position
    /// </summary>
    private void Respawn() {
        this.transform.position = this.spawnPosition;
    }
    
    private void Awake() {
        
        
        if (!this.TryGetComponent<Animator>(out this.animator)) {
            Debug.LogError("PLAYER ANIMATOR NOT FOUND");
        }
        SubscribeToEvents();
        this.spawnPosition = this.transform.position;
    }

    void OnDestroy(){
        this.UnsubscribeFromEvents();
    }

    void OnTriggerEnter2D(Collider2D Col) {
        Enemy enemy = Col.gameObject.GetComponentInParent<Enemy>();
        if (enemy != null) {
            if (enemy.IsAttacking) {
                animator.SetTrigger(AnimationTriggers.DAMAGE);
                this.PlayerHealthController.ApplyDamage(2);
            }
        }
    }
}