using System;
using UnityEngine;

/// <summary>
/// Event to trigger damage sound 
/// </summary>
public class PlayerDamagedEventArgs : EventArgs {

}
/// <summary>
/// A player is the controllable character that a person play as.
/// </summary>
public class Player : MonoBehaviour, IKillable, IDamageNotifyable {
    
    private Animator animator;

    private PlayerWeaponController playerWeaponController;

    private Vector3 spawnPosition;

    private static Player instance;

    public static event EventHandler<PlayerDamagedEventArgs> OnPlayerDamagedEvent;

    /// <summary>
    /// Respawns the player and notify that the player died
    /// </summary>
    public void IsDead() {
        Respawn();
    }

    private void SubscribeToEvents() {
        PlayerWeaponController.WeaponChangedEvent += CallbackWeaponChangedEvent;
    }

    private void UnsubscribeFromEvents() {
        PlayerWeaponController.WeaponChangedEvent -= CallbackWeaponChangedEvent;
    }

    private void CallbackWeaponChangedEvent(object _, WeaponChangedEventArgs args) {
        animator.SetInteger("ACTIVE_WEAPON", (int) args.AnimId);
    }

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

    private void OnDestroy() {
        this.UnsubscribeFromEvents();
    }

    public void Damaged() {
        animator.SetTrigger(AnimationTriggers.DAMAGE);
        PlayerDamagedEventArgs args = new PlayerDamagedEventArgs();
        OnPlayerDamagedEvent?.Invoke(this, args);
    }
}