using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour {

    [SerializeField]
    private List<GameObject> availableWeapons;

    private GameObject currentWeapon;

    private GunController activeGunController;

    // -- properties -- //
    public List<GameObject> AvailableWeapons {
        get => availableWeapons;
        set => availableWeapons = value;
    }

    public GunController ActiveGunController {
        get => activeGunController;
        internal set => this.activeGunController = value;
    }

    // -- public -- //
    // -- private -- // 

}