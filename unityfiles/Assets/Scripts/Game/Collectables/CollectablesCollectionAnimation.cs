using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesCollectionAnimation : MonoBehaviour {
    
    //The time it will take for the collectible to complete the animation
    [SerializeField] private float animationSpeed;

    private Transform coinGUIPosition;

    private Transform letterGUIPosition;
  
    // When a collectible is spawn, it will create a path to the collection point and then follow that path.
    // When the collectible is done following the path, its sprite will get disabled
    public void Start() {
        coinGUIPosition = CoinsCollecedGUI.TryGetTransform();
        letterGUIPosition = LettersCollectedGUI.TryGetTransform();
    }
    
    private void FixedUpdate() { 
        Vector3 moveCoinToCoinGUI = Vector3.MoveTowards(this.transform.position, coinGUIPosition.position, animationSpeed);
        this.transform.position = moveCoinToCoinGUI;

        Vector3 moveLetterToLetterGUI = Vector3.MoveTowards(this.transform.position, letterGUIPosition.position, animationSpeed);
        this.transform.position = moveLetterToLetterGUI;

        if (Vector2.Distance(this.transform.position, coinGUIPosition.position) < 1.5f) {
            this.gameObject.SetActive(false);
        }

        if (Vector2.Distance(this.transform.position, letterGUIPosition.position) < 1.5f) {
            this.gameObject.SetActive(false);
        }
    }
    
}
    
