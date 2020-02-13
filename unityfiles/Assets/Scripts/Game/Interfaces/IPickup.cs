using System;
using UnityEngine;

/// <summary>
/// Describes the different kind of powerups
/// </summary>
public interface IPickup{

    /// <summary>
    /// Describes how the Pickup should spawn 
    /// </summary>
    /// <param name="spawnPos">The Possision to spawn the Pickup</param>
    void Spawn(Transform spawnPos);


    /// <summary>
    /// Describes how the pickups effect should be applied. ie.
    ///     give money shoot faster, usually this will call some event
    /// </summary>
    void Activate();

}

