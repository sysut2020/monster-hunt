using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fixes the roatation of the game object the script sits on
/// Will prevent flipping from happening.
/// </summary>
public class FixedRoation : MonoBehaviour
{
    Quaternion rotation;
    void Awake()
    {
        rotation = transform.rotation;
    }
    void LateUpdate()
    {
        transform.rotation = rotation;
    }
}
