using System;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// Interface describes an pickup in the game world
/// </summary>
public interface IPickup{

    /// <summary>
    /// apply the effect to the player
    /// </summary>
    /// <param name="player">the player to apply the effect to</param>
    void ApplyEffect(Player player);

    /// <summary>
    /// returns the name of the pickup
    /// </summary>
    /// <returns>name of the pickup</returns>
    string GetPickupName();

}

