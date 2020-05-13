using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Arguments for letter collected event
/// </summary>
public class LetterCollectedArgs : EventArgs {
    public string Letter { get; set; }
}

/// <summary>
/// Used to handle the letter collectibles
/// </summary>
public class LetterCollectible : Collectible {
    public static event EventHandler<LetterCollectedArgs> OnLetterCollected;

    [SerializeField]
    private TMP_Text letterComponent;

    private MoveToGuiElement moveToGuiElement;

    private Boolean voidLetter = false;

    public string LetterString {
        get => letterComponent.text;
    }

    private void OnDestroy() {
        HuntingLevelController.OnLevelStateChangeEvent -= CallbackOnLevelStateChange;
        LetterCollectedArgs letterEventArgs = new LetterCollectedArgs();
        letterEventArgs.Letter = this.LetterString;
        if (!this.voidLetter) {
            OnLetterCollected?.Invoke(this, letterEventArgs);
        }
    }

    private void Awake() {
        HuntingLevelController.OnLevelStateChangeEvent += CallbackOnLevelStateChange;
        var generatedLetter = SudoRandomLetterGenerator.Instance.GenerateLetter();
        this.letterComponent.text = generatedLetter;
        if (TryGetComponent(out moveToGuiElement)) {
            moveToGuiElement.FindTarget<LettersCollectedGUI>();
        }
    }

    private void CallbackOnLevelStateChange(object _, LevelStateChangeEventArgs args) {
        if (args.NewState == LEVEL_STATE.GAME_WON || args.NewState == LEVEL_STATE.GAME_OVER) {
            this.voidLetter = true;
            Destroy(this.gameObject);
        }
    }
}