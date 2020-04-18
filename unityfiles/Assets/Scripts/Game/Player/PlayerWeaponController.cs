using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// event args for the weapon change event. 
/// can hold info about the new gun controller
/// </summary>
public class WeaponChangedEventArgs : EventArgs {
    public GunController NewGunController { get; set; }
    public Gun NewGun { get; set; }
    public PLAYER_ANIMATION AnimId { get; set; }

    public int GunIndex { get; set; }
}

public class PlayerWeaponController : MonoBehaviour {

    [SerializeField]
    [Tooltip("list of guns.")]
    private Gun[] availableWeapons;

    [SerializeField]
    [Tooltip("the hand that holds the gun.")]
    private GameObject gunHand;

    /// <summary>
    /// Toggled when the game is paused/unpaused
    /// </summary>
    private bool isPaused = false;

    private BulletBuffer bulletBuffer = new BulletBuffer();
    private List<GunController> gunControllers = new List<GunController>();
    private GameObject currentWeapon = null;
    private int currentWeaponIndex = -1;
    private GunController activeGunController;

    private SpriteRenderer weaponSpriteRend;

    // -- properties -- //
    public Gun[] AvailableWeapons {
        get => availableWeapons;
        set => availableWeapons = value;
    }

    public GunController ActiveGunController {
        get => activeGunController;
        internal set => this.activeGunController = value;
    }

    // -- public -- //

    public void MaybeFire() {
        this.activeGunController?.MaybeFire();
    }

    // -- events -- //

    public static event EventHandler<WeaponChangedEventArgs> WeaponChangedEvent;

    /// <summary>
    /// Called when the game is paused/unpaused.
    /// </summary>
    /// <param name="_">the object that sent the event > unused</param>
    /// <param name="args">event arguments</param>
    private void CallbackOnGamePaused(object _, GamePausedEventArgs args) {
        this.isPaused = args.IsPaued;
    }

    /// <summary>
    /// Checks for inputs from mouse/keyboard for
    /// shooting, and weapond changing
    /// </summary>
    private void CheckControlsInput() {
        if (Input.GetKeyDown(KeyCode.E)) {
            this.ChangeWeapon(1);
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            this.ChangeWeapon(-1);
        }

        if (Input.GetMouseButtonDown(0)) {
            this.MaybeFire();
        }

        if (Input.GetMouseButton(1)) {
            this.MaybeFire();
        }
    }

    /// <summary>
    /// Changes the current weapon the given number of positions
    /// If the current weapon passes one of the other indexes it loops around
    /// </summary>
    /// <param name="numChanges">the number of places in the weapon list to change the active weapon</param>
    /// <returns>the new active weapon controller</returns>
    private void ChangeWeapon(int numChanges) {
        int newIndex = (this.currentWeaponIndex + numChanges) % (this.availableWeapons.Length);
        if (newIndex < 0) { newIndex = this.availableWeapons.Length - 1; }

        if (newIndex != this.currentWeaponIndex) {
            this.currentWeaponIndex = newIndex;
            this.currentWeapon?.SetActive(false);
            this.currentWeapon = AvailableWeapons[newIndex].gameObject;
            this.activeGunController = this.gunControllers[newIndex];

            this.currentWeapon.SetActive(true);

        }

        WeaponChangedEventArgs args = new WeaponChangedEventArgs();
        args.NewGunController = this.ActiveGunController;
        args.NewGun = AvailableWeapons[newIndex];
        args.AnimId = this.activeGunController.WeaponData.HoldingAnimation;
        args.GunIndex = newIndex;

        WeaponChangedEvent?.Invoke(this, args);
    }

    // -- unity -- //

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {
        Gun[] tmp = new Gun[this.availableWeapons.Length];
        for (int i = 0; i < this.availableWeapons.Length; i++) {
            var gun = Instantiate(this.availableWeapons[i]);
            gun.transform.SetParent(this.gunHand.transform);
            gun.transform.localPosition = Vector3.zero;
            gun.transform.localScale = this.availableWeapons[i].transform.localScale;
            gun.Bullet.SetActive(false);
            GunController weaponGc = new GunController(gun, this.bulletBuffer);
            this.gunControllers.Add(weaponGc);
            gun.gameObject.SetActive(false);
            tmp[i] = gun;
        }
        this.availableWeapons = tmp;
    }

    void Start() {
        //Changes the weapon to the first weapon in inventory
        ChangeWeapon(1);
        GameManager.GamePausedEvent += CallbackOnGamePaused;
    }

    void OnDestroy() {
        GameManager.GamePausedEvent -= CallbackOnGamePaused;
    }

    /// <summary>
    /// Checks for input buttons
    /// </summary>
    private void Update() {
        if (this.isPaused) { return; }
        CheckControlsInput();
    }

}