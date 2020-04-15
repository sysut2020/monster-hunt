using System.Collections.Generic;
using UnityEngine;


// todo comment
public class BulletBuffer {
    private readonly List<GameObject> buffer = new List<GameObject>();


    /// <summary>
    /// returns the first disabled bullet in buffer
    /// if no bullets is found null is returned
    /// </summary>
    /// <returns>the first idle bullet null if none are found</returns>
    public GameObject GetBullet() {
        GameObject candidate = null;
        // loops backward for higher reuse
        if (buffer.Count > 0) {
            for (int i = buffer.Count - 1; i >= 0; i--) {
                if (!buffer[i].activeInHierarchy) {
                    candidate = buffer[i];
                    break;
                }
            }
        }


        if (candidate == null) {
            candidate = this.GenerateBullet();
        }

        return candidate;
    }

    private GameObject GenerateBullet() {
        GameObject newBullet = new GameObject();
        newBullet.name = "bullet";

        SpriteRenderer spriteRender = newBullet.AddComponent<SpriteRenderer>() as SpriteRenderer;
        BoxCollider2D boxCol = newBullet.AddComponent<BoxCollider2D>() as BoxCollider2D;
        Rigidbody2D rigidB2d = newBullet.AddComponent<Rigidbody2D>() as Rigidbody2D;
        newBullet.SetActive(false);
        buffer.Add(newBullet);

        return newBullet;
    }
}