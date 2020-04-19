using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameWon : MonoBehaviour {

    /// <summary>
    /// Time left when game ended
    /// </summary>
    private int timeLeft;

    [SerializeField]
    private TextMeshProUGUI timeLeftText;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private Button continueButton;

    /// <summary>
    /// Amount of time the countdown should take
    /// </summary>
    [SerializeField]
    private float duration = 5f;

    /// <summary>
    /// Time when the game won canvas was enabled
    /// </summary>
    private float timeEnabled;

    private void Awake() {
        if (continueButton.IsInteractable()) {
            continueButton.interactable = false;
        }
    }

    private void OnEnable() {
        GameDataManager dataManager = GameManager.Instance.GameDataManager;

        timeEnabled = Time.unscaledTime;

        timeLeft = HuntingLevelController.Instance.GetLevelTimeLeft() / 1000; // divide by 1000 to get sec
        timeLeftText.text = FormatAsClockTime(timeLeft);

        scoreText.text = "0";
        dataManager.AddGameScore(timeLeft); // add time left in seconds as points to game score
    }

    private void Update() {
        var t = (Time.unscaledTime - timeEnabled) / duration; // used to smooth the count down/up
        scoreText.text = Mathf.RoundToInt(Mathf.SmoothStep(0, timeLeft, t)).ToString();
        var timeInSeconds = Mathf.RoundToInt(Mathf.SmoothStep(timeLeft, 0, t));
        timeLeftText.text = FormatAsClockTime(timeInSeconds);

        if (timeInSeconds <= 0 && !continueButton.interactable) {
            continueButton.interactable = true;
        }
    }

    /// <summary>
    /// Returns the value as "MM:SS"
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    private string FormatAsClockTime(int seconds) {
        var min = Mathf.RoundToInt(seconds / 60);
        var sec = seconds % 60;
        var asClockTime = $"{min}:{sec}"; // string interpolation
        // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated
        if (sec < 10) {
            asClockTime = $"{min}:0{sec}"; // just added a 0 if sec is less than 10
        }

        return asClockTime;
    }
}