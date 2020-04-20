using UnityEngine;

/// <summary>
/// Used to make affected by a bullet
/// </summary>
public class MakeHitableByBullet : MonoBehaviour {

    [SerializeField]
    private EnemyHealthController affectedHealthController;

    public HealthController AffectedHealthController { get => affectedHealthController; }

    void Awake() {
        if (AffectedHealthController == null) {
            throw new MissingComponentException("Missing affected health controller");
        }
    }
}