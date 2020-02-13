using System;
using System.Linq;
using UnityEngine;

public class PlayerEventArgs: EventArgs{
    public GameObject PlayerGO {get; set;}
    public EnemyType PlayerMoney {get; set;}
    public EnemyType PlayerLetters {get; set;}
}



/// <summary>
/// this class describes the player
/// </summary>
public class Player : MonoBehaviour, IDamageable {
    

    [SerializeField]
    private HealthController playerHealthController;

    private PlayerInventory playerInventory;

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

    public HealthController PlayerHealthController {
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
    /// Checks if there exist other instances of this class.
    /// </summary>
    private void CheckSingleton() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

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

    public void Dead(){
        PlayerEventArgs args = new PlayerEventArgs();
        // TODO: fill args
        PlayerKilledEvent?.Invoke(this, args);
    }
    // -- events -- //

    public static event EventHandler<PlayerEventArgs> PlayerKilledEvent;
    private void SubscribeToEvents() {
        LevelManager.CleanUpEvent += c_CleanupEvent;
    }

    private void UnsubscribeFromEvents() {
        LevelManager.CleanUpEvent -= c_CleanupEvent;
    }

    private void c_CleanupEvent(){
        this.UnsubscribeFromEvents();
        Destroy(gameObject);
    }

    private void c_CleanupEvent(object o, EventArgs _) => this.c_CleanupEvent();

    // -- private -- // 

  

    
    /// <summary>
    /// iterates through the active effects and checks if any one of them are done
    /// if they are the effect is cleaned out and removed
    /// </summary>
    private void UpdateEffects() {
        foreach (IEffectPowerUp effect in this.playerInventory.ActivePickups.Reverse<IEffectPowerUp>()) {
            if (effect.IsEffectFinished()) {
                effect.Cleanup();
                this.playerInventory.RemoveEffectPickup(effect);
            }
        }
    }

    

    // -- unity -- //

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update() {
        UpdateEffects();
    }


    private void Awake() {
        CheckSingleton();

        animator = this.GetComponent<Animator>();
        SubscribeToEvents();
    }


    void OnTriggerEnter2D(Collider2D Col) {

        if (Col.TryGetComponent(out Enemy enemy)) {
            if (enemy.IsAttacking) {
                animator.SetTrigger(AnimationTriggers.DAMAGE);                
            }
        }
    }

}