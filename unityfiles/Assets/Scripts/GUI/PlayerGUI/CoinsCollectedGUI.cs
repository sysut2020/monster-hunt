using TMPro;
using UnityEngine;

/// <summary>
/// This class is responsible for updating the GUI with how many coins that
/// the player has collected
/// </summary>
public class CoinsCollectedGUI : MonoBehaviour {

    /// <summary>
    /// Text element for displaying collected coins
    /// </summary>
    [SerializeField]
    private TMP_Text coinCounter;

    private int collectedCoins = 0;

    private void Awake() {
        if (coinCounter == null) {
            throw new MissingComponentException("Missing text component");
        }
        CoinCollectible.OnCoinCollected += OnNewCoin;
        SetCoinamountText();
    }

    private void SetCoinamountText() {
        this.coinCounter.SetText($"{collectedCoins}");
    }

    private void OnNewCoin(object sender, CoinCollectedArgs coin) {
        this.collectedCoins++;
        this.SetCoinamountText();
    }

    private void OnDestroy() {
        CoinCollectible.OnCoinCollected -= OnNewCoin;
    }

}