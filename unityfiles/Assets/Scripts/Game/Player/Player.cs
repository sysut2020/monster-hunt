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

    private PlayerInventory playerInventory = new PlayerInventory();

    private Animator animator;

    public delegate void EventHandler();

    public static event EventHandler OnPlayerDead;

    private PlayerWeaponController playerWeaponController;

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

    public PlayerInventory PlayerInventory {
        get => playerInventory;
        internal set => this.playerInventory = value;
    }

    public PlayerWeaponController PlayerWeaponController {
        get => playerWeaponController;
        internal set => this.playerWeaponController = value;
    }

    // -- public -- //

    /// <summary>
    /// Gives a pickup to the player
    /// </summary>
    /// <param name="pickup">the pickup to give to the player</param>
    public void GivePickup(IPowerUp pickup) {
        // activate
        pickup.ApplyEffect(this);
        // if effect is persistent give it to the watcher
        if (pickup is IEffectPowerUp) {
            IEffectPowerUp equalTypeEffect = playerInventory.ActivePickups.Find(effect => effect.GetPickupName().Equals(pickup.GetPickupName()));
            if (equalTypeEffect != null) {
                // if it exists extend the existing
                equalTypeEffect.ExtendEffect(equalTypeEffect);
            } else {
                // else just bop it on the end
                this.PlayerInventory.AddEffectPickup(pickup as IEffectPowerUp);
            }
        }
    }

    public void Dead() {
        PlayerEventArgs args = new PlayerEventArgs();
        // TODO: fill args
        PlayerKilledEvent?.Invoke(this, args);
    }
    // -- events -- //

    public static event EventHandler<PlayerEventArgs> PlayerKilledEvent;
    private void SubscribeToEvents() {
        LevelManager.CleanUpEvent += this.c_CleanupEvent;
        PlayerWeaponController.WeaponChangedEvent += c_WeaponChangedEvent;
;
    }

    private void UnsubscribeFromEvents() {
        LevelManager.CleanUpEvent -= this.c_CleanupEvent;
        PlayerWeaponController.WeaponChangedEvent -= c_WeaponChangedEvent;
;
    }

    private void c_CleanupEvent() {
        this.UnsubscribeFromEvents();
        Destroy(gameObject);
    }

    private void c_CleanupEvent(object o, EventArgs _) => c_CleanupEvent();


    private void c_WeaponChangedEvent(object _, WeaponChangedEventArgs args){
        animator.SetInteger("ACTIVE_WEAPON", (int)args.AnimId);
    }

    // -- private -- // 

    /// <summary>
    /// iterates through the active effects and checks if any one of them are done
    /// if they are the effect is cleaned out and removed
    /// </summary>
    private void UpdateEffects() {
        if (this.playerInventory.ActivePickups.Count > 0) {
            List<IEffectPowerUp> tmp = this.playerInventory.ActivePickups;
            tmp.Reverse<IEffectPowerUp>();
            foreach (IEffectPowerUp effect in tmp) {
                if (effect.IsEffectFinished()) {
                    effect.Cleanup();
                    this.playerInventory.RemoveEffectPickup(effect);
                }
            }
        }

    }

    // -- unity -- //

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update() {
        //UpdateEffects();
    }

    private void Awake() {
        if (!this.TryGetComponent<Animator>(out this.animator)) {
            Debug.LogError("PLAYER ANIMATOR NOT FOUND");
        }
        SubscribeToEvents();
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