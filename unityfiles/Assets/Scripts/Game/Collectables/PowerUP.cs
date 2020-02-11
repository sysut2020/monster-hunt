using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUP : MonoBehaviour {
    private IEffectPickup effectPickup;
    public IEffectPickup EffectPickup {
        get => effectPickup;
        set => effectPickup = value;
    }
    
    
}
