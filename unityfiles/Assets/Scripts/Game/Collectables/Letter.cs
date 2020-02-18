using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Arguments for letter collected event
/// </summary>
public class LetterCollectedArgs : EventArgs {
    public string Letter { get; set; }
}

public class Letter : MonoBehaviour {
    public static event EventHandler<LetterCollectedArgs> OnLetterCollected;

    private string letterString;
    public string LetterString {
        get => letterString;
        set => letterString = value;
    }

    private void OnDestroy() {
        LetterCollectedArgs letterEventArgs = new LetterCollectedArgs();
        letterEventArgs.Letter = this.letterString;
        this.TriggerLetterCollected(letterEventArgs);
    }

    private void TriggerLetterCollected(LetterCollectedArgs letterEventArgs) {
        Letter.OnLetterCollected?.Invoke(this, letterEventArgs);

    }
}