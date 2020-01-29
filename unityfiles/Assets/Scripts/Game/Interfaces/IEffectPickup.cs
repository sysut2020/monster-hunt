using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface describes a pickup in the world with an effect that lasts over a 
/// set amount of time
/// </summary>
public interface IEffectPickup: IPickup{

    /// <summary>
    /// This method is called when an instance of the effect already exists an another one is added.
    /// This method defines how to apply another effect on top of itself.
    /// ie. should the sum or the product of an increse in fire rate be used. that is defined here
    /// </summary>
    /// <param name="extender">an instance of the same objet to extend the effect with</param>
    void ExtendEffect(IEffectPickup extender);

    /// <summary>
    /// checks this effect is active in any way
    /// </summary>
    /// <returns>returns true if it is false if not</returns>
    bool IsEffectFinished();

    /// <summary>
    /// Returns all the variables back to how they initially were
    /// </summary>
    void Cleanup();

}

