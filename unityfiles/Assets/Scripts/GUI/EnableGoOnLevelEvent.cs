using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utillity class used to enable/disable a     game object 
/// on an event sent from the level manager
/// Manly used to enable/disable gui elements
/// </summary>
public class EnableGoOnLevelEvent : MonoBehaviour {

    [SerializeField]
    private LEVEL_STATE enableEvent;

    [SerializeField]
    private LEVEL_STATE disableEvent;

    [SerializeField]
    private GameObject toDisable;

    private void SubscribeToEvents() {
        HuntingLevelController.OnLevelStateChangeEvent += CallbackLevelStateChange;
    }

    private void UnsubscribeFromEvents() {
        HuntingLevelController.OnLevelStateChangeEvent -= CallbackLevelStateChange;
    }

    private void CallbackLevelStateChange(object _, LevelStateChangeEventArgs args) {
        if (args.NewState == enableEvent) {
            this.toDisable.SetActive(true);
        } else if (args.NewState == disableEvent) {
            this.toDisable.SetActive(false);
        }
    }
    void Awake() {
        if (toDisable == null) {
            throw new MissingComponentException("enabler missing somthing to enable");
        }
        SubscribeToEvents();
    }

    void OnDestroy() {
        UnsubscribeFromEvents();
    }
}