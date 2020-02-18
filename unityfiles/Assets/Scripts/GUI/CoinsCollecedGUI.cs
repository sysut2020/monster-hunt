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
    
    private static Vector3 resultPosition;
    
    private int collectedCoins = 0;
   
    private static RectTransform myRectTransform;

    private static GameObject coinGUIObject;

    private void Awake() {
        if (coinCounter == null) throw new MissingComponentException("Missing text component");
        CollectableEvents.OnCoinCollected += OnNewCoin;
        coinGUIObject = new GameObject("World coin position");
        SetCoinamountText();
        TryGetComponent<RectTransform>(out myRectTransform);
    }

    private void FixedUpdate() {
        Vector2 vectorRectTransformPosition = myRectTransform.transform.position;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(myRectTransform, vectorRectTransformPosition, FindObjectOfType<Camera>(), out resultPosition);
        coinGUIObject.transform.position = resultPosition;
    }


    private void SetCoinamountText() {
        this.coinCounter.SetText($"{collectedCoins}");
    }

    private void OnNewCoin(object sender, CoinCollectedArgs coin) {
        this.collectedCoins++;
        this.SetCoinamountText();
    }

    private void OnDestroy() {
        CollectableEvents.OnCoinCollected -= OnNewCoin;
    }

    public static Transform TryGetTransform() {
        return coinGUIObject.transform;
    }
}