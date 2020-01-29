using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// interface describes a pickup that affects the gun in som way
/// </summary>
public interface IWeaponEffectPickup : IEffectPickup {

    /// <summary>
    /// Describes how to react when the active weapon is changed
    /// </summary>
    /// <param name="newGunC">the weapon controller of the new gun</param>
    void OnChangeWeapon (GunController newGunC);

}