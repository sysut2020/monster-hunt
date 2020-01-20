using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlayerMovement : MonoBehaviour {

    public Rigidbody2D rigidBodyPlayer;
    public float speed = 100;
    public GameObject ground;

    void Update(){
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Collision collision = new Collision();
        //Debug.Log(horizontal);
        //Vector2 tempVector = new Vector2(horizontal, vertical);
        //tempVector = tempVector.normalized * speed * Time.deltaTime;
        if (horizontal < 0 || horizontal > 0) {
            rigidBodyPlayer.MovePosition(rigidBodyPlayer.transform.position + new Vector3(horizontal, vertical, this.transform.position.z) * Time.deltaTime);
        }

        if (vertical > 0 || horizontal < 0) {
            float jump = 1000;
            rigidBodyPlayer.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        }
    }
}
