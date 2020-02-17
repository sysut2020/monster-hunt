using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// This class is responsible for updating the health bar, displaying
/// amount of health as a pool of health, which increase / decrease as health
/// changes.
/// </summary>
public class HealthbarGUI : MonoBehaviour {

    /// <summary>
    /// Maximum health so we know what is maximum
    /// </summary>
    private float maxHealth = 0;

    /// <summary>
    /// Current health value
    /// </summary>
    private float currentHealth = 0;

    public float CurrentHealth {
        get { return this.currentHealth; }
        set { this.currentHealth = value; }
    }

    private void Awake() {
        // if (coinCounter == null) throw new MissingComponentException("Missing text component");
        // CollectableEvents.OnCoinCollected += OnHealthChanged;
    }

    private void UpdateHealthPool() {

    }

    private void OnHealthChanged(object sender, int healthChange) {
        this.CurrentHealth = healthChange;
        this.UpdateHealthPool();
    }

    private void OnDestroy() {
        // CollectableEvents.OnCoinCollected -= OnHealthChanged;
    }
}