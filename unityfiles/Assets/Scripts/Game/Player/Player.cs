using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// this class describes the player
/// </summary>
public class Player : MonoBehaviour, IDamageable {
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

    [SerializeField]
    private HealthController playerHealthController;

    private PlayerInventory playerInventory;

    private Animator animator;

    public delegate void EventHandler();

    public static event EventHandler OnPlayerDead;


    private PlayerWeaponController playerWeaponController;

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

    private void Awake() {
        CheckSingleton();

        animator = this.GetComponent<Animator>();
        SubscribeToEvents();
    }

    private void OnDestroy() {
        UnsubscribeFromEvents();
    }
    
    private void SubscribeToEvents() {
        GameManager.OnEndGame += EndGame;
    }

    private void UnsubscribeFromEvents() {
        GameManager.OnEndGame -= EndGame;
    }

    private void EndGame() {
        Destroy(gameObject);
    }

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

    /// <summary>
    /// Gives money to the player
    /// </summary>
    /// <param name="money">the amount of money to give the player</param>
    public void GiveMoney(int money) {
        this.PlayerInventory.AddMoney(money);
    }

    /// <summary>
    /// Gives a letter to the player
    /// </summary>
    /// <param name="letter">the letter to give to the player</param>        
    public void GiveLetter(String letter) {
        this.playerInventory.AddLetter(letter);
    }


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

    /// <summary>
    /// Checks for collisions with other objects
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other) {
        var enemy = other.gameObject.GetComponent<Enemy>();

        try {
            if (enemy.IsAttacking) {
                animator.SetTrigger(AnimationTriggers.DAMAGE);                
            }
        } catch (System.Exception) { }

    }
}