using UnityEngine;

/// <summary>
/// listens for enemy sounds to be played  
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