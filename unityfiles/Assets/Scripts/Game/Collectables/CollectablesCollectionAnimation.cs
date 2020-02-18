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
    /// The speed it will use to fly to the location
    /// </summary>
    [SerializeField]
    [Range(0, 2)]
    private float speed;

    private Transform pos;
    // When a collectible is spawn, it will create a path to the collection point and then follow that path.
    // When the collectible is done following the path, its sprite will get disabled
    public void Start() {
        pos = CoinsCollecedGUI.TryGetTransform();
    }

    private void FixedUpdate() {
        Vector3 newPosition = Vector3.MoveTowards(this.transform.position, pos.position, speed);
        if ((this.transform.position - pos.position).magnitude < 0.5f) {
            GameObject.Destroy(this.gameObject);
        } else {
            this.transform.position = newPosition;
        }

        // if (Vector2.Distance(this.transform.position, letterGUIPosition.position) < 1.5f) {
        //     this.gameObject.SetActive(false);
        // }
    }

}