using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for displaying the amount of lives a player has.
/// Lives can be added and removed.
/// It listens on PlayerLivesUpdate, and adds/remove lives visuals as lives changes.
/// </summary>
public class LivesGUI : MonoBehaviour {

    /// <summary>
    /// The object that represents lives in the GUI; that will be displayed
    /// </summary>
    [SerializeField]
    private Transform lifeObject;

    readonly List<Transform> lifesObjects = new List<Transform>();

    /// <summary>
    /// Flag that tells if lives are generated on first try.
    /// </summary>
    private bool initialized = false;

    private void Awake() {
        PlayerHealthController.OnPlayerLivesUpdate += CallbackOnLivesUpdate;
    }

    // Start is called before the first frame update
    void Start() {
        if (this.lifeObject == null) {
            throw new MissingReferenceException("Please provide a reference to a life object to display.");
        }
    }

    private void OnDestroy() {
        PlayerHealthController.OnPlayerLivesUpdate -= CallbackOnLivesUpdate;
    }

    /// <summary>
    /// Adds or removes lives when player lives updates.
    /// </summary>
    /// <param name="_">unsused</param>
    /// <param name="args">player lives argument</param>
    private void CallbackOnLivesUpdate(object _, PlayerLivesUpdateArgs args) {
        if (!initialized) {
            GenerateLifeObject(args.CurrentLives);
            initialized = true;
        } else {
            var heartsLeft = this.lifesObjects.FindAll(life => life.gameObject.activeSelf).Count;
            if (heartsLeft < args.CurrentLives) {
                this.AddLife(args.CurrentLives - heartsLeft);
            } else if (heartsLeft > args.CurrentLives) {
                this.RemoveLife(heartsLeft - args.CurrentLives);
            }
        }
    }

    /// <summary>
    /// Generates provided amount of lives objects and adds them as its
    /// siblings.
    /// </summary>
    public void GenerateLifeObject(int lives) {
        for (int i = 0; i < lives; i++) {
            var life = Instantiate(lifeObject);
            life.SetParent(this.transform, false);
            this.lifesObjects.Add(life);
        }
    }

    /// <summary>
    /// Adds provided amount of lives.
    /// </summary>
    /// <param name="amount">number of lives to add</param>
    private void AddLife(int amount) {
        int totalLives = this.lifesObjects.Count;

        int addedCounter = 0;

        for (int i = 0; i < totalLives; i++) {
            var go = this.lifesObjects[i].gameObject;
            if (!go.activeSelf && addedCounter < amount) {
                go.SetActive(true);
                addedCounter++;
            }
        }

        if (addedCounter < amount) {
            this.GenerateLifeObject(amount - addedCounter);
        }
    }

    /// <summary>
    /// Remove number of lives.
    /// </summary>
    /// <param name="amount">amount of lives to remove</param>
    private void RemoveLife(int amount) {
        int totalLives = this.lifesObjects.Count;

        int removedCounter = 0;

        for (int i = 0; i < totalLives; i++) {
            var go = this.lifesObjects[i].gameObject;
            if (go.activeSelf && removedCounter < amount) {
                go.SetActive(false);
                removedCounter++;
            }

        }
    }

}