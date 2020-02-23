using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// event args for the weapon change event. 
/// can hold info about the new gun controller
/// </summary>
public class WeaponChangedEventArgs: EventArgs{
    public GunController NewGunController {get; set;}
    public PLAYER_ANIMATION AnimId {get; set;}
}

public class PlayerWeaponController: Singleton<PlayerWeaponController> {

    [SerializeField]
    [Tooltip("list of guns.")]
    private List<WeaponData> availableWeapons;

    [SerializeField]
    [Tooltip("the hand that holds the gun.")]
    private GameObject gunHand;

    [SerializeField]
    private GameObject firePoint;
    [SerializeField]
    private GameObject weaponGO;

    



    private WeaponData currentWeapon;
    private int currentWeaponIndex = -1;
    private GunController activeGunController;



    
    private SpriteRenderer weaponSpriteRend;


    
    
    // -- properties -- //
    public List<WeaponData> AvailableWeapons {
        get => availableWeapons;
        set => availableWeapons = value;
    }

    public GunController ActiveGunController {
        get => activeGunController;
        internal set => this.activeGunController = value;
    }
    public GameObject FirePoint { get => firePoint; set => firePoint = value; }

    // -- public -- //

    /// <summary>
    /// Starts to fire the weapon
    /// </summary>
    public void StartFiring() {this.activeGunController.StartFiring();}

    /// <summary>
    /// Stop fireing the weapon
    /// </summary>
    public void StopFiring() {this.activeGunController.StopFiring();}

    /// <summary>
    /// Fires a bullet if able (not on cooldown since last shot)
    /// </summary>
    public void FireOnce() {this.activeGunController.FireOnce();}

    /// <summary>
    /// changes to the next weapon in the weapon list 
    /// if the end of the list is reached the index loops around
    /// </summary>
    /// <returns>The weapon controller of the new active weapon</returns>
    //public GunController ChangeToNextWeapon() => this.ChangeWeapon(1);


    /// <summary>
    /// changes to the prev weapon in the weapon list 
    /// if the end of the list is reached the index loops around
    /// </summary>
    /// <returns>The weapon controller of the new active weapon</returns>
    //public GunController ChangeToPrevWeapon() => this.ChangeWeapon(-1);

    // -- events -- //

    public static event EventHandler<WeaponChangedEventArgs> WeaponChangedEvent;
    
    // -- private -- //

    /// <summary>
    /// Changes the current weapon the given number of positions
    /// If the current weapon passes one of the other indexes it loops around
    /// </summary>
    /// <param name="numChanges">the number of places in the weapon list to change the active weapon</param>
    /// <returns>the new active weapon controller</returns>
    private void ChangeWeapon(int numChanges){
        int newIndex = (this.currentWeaponIndex + numChanges) % (this.availableWeapons.Count);
        if (newIndex < 0){newIndex=this.availableWeapons.Count -1;}

        if (newIndex != this.currentWeaponIndex){
            this.currentWeaponIndex = newIndex;
            this.currentWeapon = AvailableWeapons[newIndex];

            this.weaponSpriteRend.sprite = currentWeapon.WeaponSprite;

            this.weaponGO.transform.localPosition = currentWeapon.WeaponPosission;
            this.weaponGO.transform.localRotation = Quaternion.Euler(currentWeapon.WeaponRotation);
            this.weaponGO.transform.localScale = currentWeapon.WeaponScale;

            this.FirePoint.transform.localPosition = currentWeapon.FPPosission;
            this.FirePoint.transform.localRotation = Quaternion.Euler(currentWeapon.FPRotation);

            //TODO: Change not destroy
            Destroy(ActiveGunController);
            
            this.ActiveGunController = this.FirePoint.AddComponent<GunController>() as GunController;

            this.activeGunController.FirePoint = this.firePoint;
            this.ActiveGunController.BulletSprite = currentWeapon.BulletSprite;
            this.ActiveGunController.BulletDamage = currentWeapon.BulletDamage;
            this.ActiveGunController.BulletTtl = currentWeapon.BulletTtl;
            this.ActiveGunController.BulletVelocity = new Vector2(currentWeapon.BulletVelocity, 0);

        }
        WeaponChangedEventArgs args = new WeaponChangedEventArgs();
        args.NewGunController = this.ActiveGunController;
        args.AnimId = this.currentWeapon.HoldingAnimation;

        WeaponChangedEvent?.Invoke(this, args);
    }

    // -- unity -- //

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake(){
        //this.weaponGO = new GameObject();
        this.weaponSpriteRend = weaponGO.AddComponent<SpriteRenderer>() as SpriteRenderer;

        //this.FirePoint = new GameObject();

        //weaponGO.transform.parent = gunHand.transform;
        //FirePoint.transform.parent = weaponGO.transform;

        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            ChangeWeapon(1);
        }
    }


    


}