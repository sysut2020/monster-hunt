using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for any object that can receive damage
/// </summary>
public interface IDamageable {

    /// <summary>
    /// describes what to do when the character dies
    /// </summary>
    void Dead();
}