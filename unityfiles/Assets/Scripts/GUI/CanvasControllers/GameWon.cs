using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handler for the game won canvas.
/// The canvas will count down the time remaining and add it to the total score.
/// Will display the count down and score updating in real time to the user.
/// Enables the continue button when the count down is done
/// </summary>
public class GameWon : MonoBehaviour {
    /// <summary>
    /// Time left when game ended
    /// </summary>
    private int timeLeft;

    [SerializeField]
    private TextMeshProUGUI timeLeftText;

    [SerializeField]
    private TextMeshProUGUI extraScore;

    [SerializeField]
    private TextMeshProUGUI gameScore;

    [SerializeField]
    private TextMeshProUGUI totalScore;

    [SerializeField]
    private Button continueButton;

    private int levelScore;

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

        extraScore.text = "0";

        timeEnabled = Time.unscaledTime;
        timeLeft = HuntingLevelController.Instance.GetLevelTimeLeft() / 1000; // divide by 1000 to get sec
        timeLeftText.text = FormatAsClockTime(timeLeft);

        this.levelScore = dataManager.GameScore;
        gameScore.text = this.levelScore.ToString();
        dataManager.AddGameScore(timeLeft); // add time left in seconds as points to game score
    }

    private void Update() {
        var t = (Time.unscaledTime - timeEnabled) / duration; // used to smooth the count down/up
        var pointsCounter = Mathf.RoundToInt(Mathf.SmoothStep(0, timeLeft, t));
        extraScore.text = pointsCounter.ToString();
        var timeInSeconds = Mathf.RoundToInt(Mathf.SmoothStep(timeLeft, 0, t));
        timeLeftText.text = FormatAsClockTime(timeInSeconds);
        totalScore.text = (this.levelScore + pointsCounter).ToString();

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
            asClockTime = $"{min}:0{sec}"; // just added a leading 0 if sec is less than 10
        }

        return asClockTime;
    }
}