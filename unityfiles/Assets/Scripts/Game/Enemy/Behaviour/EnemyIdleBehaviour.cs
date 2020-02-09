using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defining the enemy behaviour of the idle state.
/// This is also the entry point of any new enemies
/// </summary>
public class EnemyIdleBehaviour : StateMachineBehaviour {

    /// <summary>
    /// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    /// Here, it automatically changes to patrol state
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool("patrol", true);
    }
}