using UnityEngine;

/// <summary>
/// listens for enemy state events, then to play the sound corresponding to that event
/// </summary>
public class EnemyAudioListner : AudioListner {
    [SerializeField]
    private Sound attackPlayerSound;

    private void Awake() {
        SubscribeToEvents();
    }

    private void SubscribeToEvents() {
        EnemyBehaviour.EnemyBehaviourStateChangeEvent += CallbackEnemyBehaviourStateChangeEvent;
    }

    private void CallbackEnemyBehaviourStateChangeEvent(object o, EnemyBehavourChangeArgs args) {
        if (args.NewBehaviourState == EnemyBehaviour.BehaviourState.ATTACK) {
            PlaySound(attackPlayerSound);
        }
    }
}