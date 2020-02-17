using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesGUI : MonoBehaviour {

    /// <summary>
    /// The object that represents lives in the GUI; that will be displayed
    /// </summary>
    [SerializeField]
    private Transform lifeObject;

    private List<Transform> lifesObjects = new List<Transform>();

    // Start is called before the first frame update
    void Start() {
        if (this.lifeObject == null) throw new MissingReferenceException("Please provide a reference to a life object to display.");
        this.GenerateLifeObject(3);
    }

    /// <summary>
    /// Generates provided amount of lives objects and adds them as its
    /// siblings. All references are stored in an ArrayList
    /// </summary>
    public void GenerateLifeObject(int lives) {
        for (int i = 0; i < lives; i++) {
            var life = Instantiate(lifeObject);
            life.SetParent(this.transform, false);
            this.lifesObjects.Add(life);
        }
    }

    /// <summary>
    /// Enables lives if there are any diabled, else create remaining
    /// </summary>
    /// <param name="amount">number of lives to add</param>
    public void AddLife(int amount) {
        int totalLives = this.lifesObjects.Count;

        int addedCounter = 0;

        for (int i = 0; i < totalLives; i++) {
            var go = this.lifesObjects[i].gameObject;
            if (!go.active && addedCounter < amount) {
                go.SetActive(true);
                addedCounter++;
            }
        }

        if (addedCounter < amount) {
            this.GenerateLifeObject(amount - addedCounter);
        }
    }

    /// <summary>
    /// Remove number of lives from the GUI
    /// </summary>
    /// <param name="amount">amount of lives to remove</param>
    public void RemoveLife(int amount) {
        int totalLives = this.lifesObjects.Count;

        int removedCounter = 0;

        for (int i = 0; i < totalLives; i++) {
            var go = this.lifesObjects[i].gameObject;
            if (go.active && removedCounter < amount) {
                go.SetActive(false);
                removedCounter++;
            }

        }
    }

}