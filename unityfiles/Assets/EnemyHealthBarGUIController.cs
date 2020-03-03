using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Takes care of displaying and updating the enemy's health bar
/// </summary>
public class EnemyHealthBarGUIController : MonoBehaviour {
    [SerializeField]
    private GameObject healthBarBackground;

    private float startHealth;

    [SerializeField]
    private Image healthBar;


    public void SetStartHealth(float value) {
        startHealth = value;
    }

    // Start is called before the first frame update
    void Start() {
        HideHealthBar();
    }

    private void HideHealthBar() {
        healthBarBackground.SetActive(false);
    }

    private void DisplayHealthBar() {
        healthBarBackground.SetActive(true);
    }

    /// <summary>
    /// Updates the healthbar to display the remaining health as a fraction of the start health
    /// </summary>
    /// <param name="health">how much the health the enemy currently has</param>
    public void UpdateHealthBar(float health) {
        if (health <= 0) return; // if health is below 0 there is no point in updating health bar
        if (!healthBarBackground.activeSelf) {
            // if the health bar is not shown, show it.
            DisplayHealthBar();
        }

        healthBar.fillAmount = health / startHealth;
    }
}