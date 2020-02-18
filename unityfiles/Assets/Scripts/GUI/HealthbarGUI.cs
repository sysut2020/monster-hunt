﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public float MaxHealth {
        get { return this.maxHealth; }
        set { this.maxHealth = value; }
    }

    /// <summary>
    /// Current health value
    /// </summary>
    private float currentHealth = 0;

    public float CurrentHealth {
        get { return this.currentHealth; }
        set { this.currentHealth = value; }
    }

    /// <summary>
    /// Image that represents the healthpool.
    /// </summary>
    [SerializeField]
    private Image healthpool;

    /// <summary>
    /// Initializes the healthpool image as a filled type, and horizontal fill
    /// and origins to the left, so when decreasing health, it decreses towards
    /// the left.
    /// </summary>
    private void SetupHealthpool() {
        healthpool.type = Image.Type.Filled;
        healthpool.fillMethod = Image.FillMethod.Horizontal;
        healthpool.fillOrigin = 0; // LEFT - 1 is RIGHT
    }

    private void Awake() {
        if (this.healthpool == null) throw new MissingComponentException("Please provide an image for the healthpool");
        this.SetupHealthpool();
    }

    private void UpdateHealthPool() {
        this.healthpool.fillAmount = this.CurrentHealth / this.MaxHealth;
    }

    private void OnDestroy() {
        // NEEDED LATER FOR EVENTS
    }
}