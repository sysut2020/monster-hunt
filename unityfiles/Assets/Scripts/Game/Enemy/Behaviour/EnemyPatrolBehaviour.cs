using Rnd = System.Random;
using UnityEngine;

/// <summary>
/// Behaviour controlling the patrolling state.
/// The Enemy will patrol the scene in order to hunt the player
/// </summary>
public class EnemyPatrolBehaviour : StateMachineBehaviour {

    [SerializeField]
    private float speed = 1;

    [SerializeField]
    private float maxVisionLength = 10f;

    private void StartChasingPlayer(Animator animator) {
        animator.SetBool("chase", true);
        animator.SetBool("patrol", false);
    }

    float lastPatrolChange = 0f;

    float patrolTime = 2;

    Transform parentTransform;

    /// <summary>
    /// Utilizes its vision to detect if a player is in front of the enemy.
    /// The enemy is only detecting the visible object in front of him, 
    /// therefore a wall, big stone or anything in between should not 
    /// make the player visible
    /// </summary>
    /// <returns>True if player is visble to enemy, false if not visible</returns>
    private bool SearchForPlayer(Transform enemy) {
        bool playerFound = false;
        Vector2 enemyFrontPoint = enemy.GetComponent<Enemy>().FrontPoint.position;

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
        } else {
            Patrol();
        }
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        this.parentTransform = animator.transform.parent;
    }

    /// <summary>
    /// Patrol movement, selects a random patrol time, and speed
    /// on each patrol trip
    /// </summary>
    private void Patrol() {
        float currentTime = Time.time;
        if (currentTime - lastPatrolChange > patrolTime) {
            this.parentTransform.Rotate(0, 180, 0);
            lastPatrolChange = currentTime;
            Rnd rnd = new Rnd();
            this.speed = rnd.Next(15, 25) / 10;
            this.patrolTime = this.speed = rnd.Next(20, 41) / 10;
        }
        this.parentTransform.Translate(Vector3.right * Time.deltaTime * speed);
    }

}