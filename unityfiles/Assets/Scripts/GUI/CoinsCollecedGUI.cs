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

    public Camera MainCam;
    private static Vector3 result;

    private int collectedCoins = 0;

    public static RectTransform myRect;

    private static GameObject go;

    private void Awake() {
        if (coinCounter == null) throw new MissingComponentException("Missing text component");
        Coin.OnCoinCollected += OnNewCoin;
        go = new GameObject("World coin pos");
        SetCoinamountText();
        TryGetComponent<RectTransform>(out myRect);
        Debug.Log(myRect.position);
    }

    private void FixedUpdate() {
        Vector2 myV2 = myRect.transform.position;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(myRect,
            myV2,
            FindObjectOfType<Camera>(),
            out result);
        go.transform.position = result;
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
        return go.transform;
    }
}