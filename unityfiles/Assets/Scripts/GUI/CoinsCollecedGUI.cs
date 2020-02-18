using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for updating the GUI with how many coins that 
/// the player has collected
/// </summary>
public class CoinsCollecedGUI : MonoBehaviour {

    /// <summary>
    /// Text for visualizing numbers of letters collected and have left.
    /// </summary>
    [SerializeField]
    TMP_Text coinCounter;
    
    private int collectedCoins = 0;
    
    private static Vector3 convertedPosition;

    private static RectTransform myRectTransform;

    private static GameObject coinGUIObject;

    private void Awake() {
        if (coinCounter == null) throw new MissingComponentException("Missing text component");
        Coin.OnCoinCollected += OnNewCoin;
        go = new GameObject("World coin pos");
        SetCoinamountText();
        TryGetComponent<RectTransform>(out myRectTransform);
    }
    
    /// <summary>
    /// Used to convert the GUI position of the coin GUI tab at the games camera,
    /// to its position in game.
    /// </summary>
    private void FixedUpdate() {
        Vector2 myV2 = myRect.transform.position;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(myRect,
            myV2,
            FindObjectOfType<Camera>(),
            out result);
        go.transform.position = result;
        coinGUIObject.transform.position = convertedPosition;
    }

    private void SetCoinamountText() {
        this.coinCounter.SetText($"{collectedCoins}");
    }

    private void OnNewCoin(object sender, CoinCollectedArgs coin) {
        this.collectedCoins++;
        this.SetCoinamountText();
    }

    private void OnDestroy() {
        Coin.OnCoinCollected -= OnNewCoin;
    }

    public static Transform TryGetTransform() {
        return coinGUIObject.transform;
    }
}