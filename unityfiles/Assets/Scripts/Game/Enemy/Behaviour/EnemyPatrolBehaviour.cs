using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour controlling the patrolling state.
/// The Enemy will patrol the scene in order to hunt the player
/// </summary>
public class EnemyPatrolBehaviour : StateMachineBehaviour {

    // The front and rear vision points from enemy object
    private Vector2 enemyFrontPoint;
    private Vector2 enemyRearPoint;

    [SerializeField]
    private float speed = 1;

    [SerializeField]
    private float maxVisionLength = 10f;

    private void StartChasingPlayer(Animator animator) {
        animator.SetBool("chase", true);
        animator.SetBool("patrol", false);
    }

    /// <summary>
    /// Utilizes its vision to detect if a player is in front of the enemy.
    /// The enemy is only detecting the visible object in front of him, 
    /// therefore a wall, big stone or anything in between should not 
    /// make the player visible
    /// </summary>
    /// <returns>True if player is visble to enemy, false if not visible</returns>
    private bool SearchForPlayer(Transform enemy) {
        bool playerFound = false;
        enemyFrontPoint = enemy.GetComponent<Enemy>().FrontPoint.position;

        // search for player based on rotation of object
        Vector2 direction = (enemy.rotation.eulerAngles.y < 90) ? Vector2.right : Vector2.left;
        RaycastHit2D hitForward = Physics2D.Raycast(enemyFrontPoint, direction, maxVisionLength);

        if (hitForward.collider != null) {
            if (hitForward.transform.tag == "Player") {
                playerFound = true;
            }
        }

        return playerFound;
    }

    /// <summary>
    /// Updates the state by continually checking if the player is visible for the enemy.
    /// If it is, it will start chasing the player
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (SearchForPlayer(animator.transform.parent)) {
            StartChasingPlayer(animator);
        }
    }
}