using TMPro;
using UnityEngine;

public class LetterController : MonoBehaviour {

    [SerializeField]
    private TMP_Text letterComponent;

    /// <summary>
    /// Assigns the new letter to the component
    /// </summary>
    /// <param name="letter">The new letter to assign</param>
    public void SetLetter(string letter) {
        letterComponent.SetText(letter);
    }

    /// <summary>
    /// Returns the letter from the letter text mesh pro
    /// </summary>
    /// <param name="letter">The letter on the stone brick</param>
    /// <returns>The letter</returns>
    public string GetLetter(string letter) {
        return letterComponent.text;
    }
}