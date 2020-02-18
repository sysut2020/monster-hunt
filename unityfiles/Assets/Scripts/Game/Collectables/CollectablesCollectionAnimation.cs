using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesCollectionAnimation : MonoBehaviour {
    
    // The speed of moving collectibles to inventory animation 
    [SerializeField] private float animationSpeed;
    // The GUI position of the coin tab
    private Transform coinGUIPosition;
    // The GUI position of the letter tab
    private Transform letterGUIPosition;

    /// <summary>
    /// When a collectible spawns, we need to know
    /// the transform position of the GUI tabs.
    /// </summary>
    public void Start() {
        coinGUIPosition = CoinsCollecedGUI.TryGetTransform();
        letterGUIPosition = LettersCollectedGUI.TryGetTransform();
    }
    
    /// <summary>
    /// Moves each collectible to their respective GUI tab.
    /// When the collectible is close enough to their respective GUI tab,
    /// that collectible will get disabled.
    /// </summary>
    private void FixedUpdate() { 
        Vector3 moveCoinToCoinGUI = Vector3.MoveTowards(this.transform.position, coinGUIPosition.position, animationSpeed);
        this.transform.position = moveCoinToCoinGUI;

        Vector3 moveLetterToLetterGUI = Vector3.MoveTowards(this.transform.position, letterGUIPosition.position, animationSpeed);
        this.transform.position = moveLetterToLetterGUI;
        
        CheckDistanceBoundary();
    }
    
    /// <summary>
    /// Checks if a collectible is inside a certain boundary
    /// </summary>
    private void CheckDistanceBoundary() {
        if (Vector2.Distance(this.transform.position, coinGUIPosition.position) < 1.5f) {
            this.gameObject.SetActive(false);
        }

        if (Vector2.Distance(this.transform.position, letterGUIPosition.position) < 1.5f) {
            this.gameObject.SetActive(false);
        }
    }
    
}
    
