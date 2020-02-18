using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SE = UnityEngine.SceneManagement;

public class Reset : MonoBehaviour {

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            var sc = SE.SceneManager.GetActiveScene().buildIndex;
            SE.SceneManager.LoadScene(sc);
        }
    }
}