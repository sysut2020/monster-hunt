using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowBehaviour : StateMachineBehaviour {

    private Transform player;

    private bool facingRight;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float attackReach = 2;

    [SerializeField]
    private float maxVisionLength = 10;

    private Enemy enemy;

    private Transform parent;

    bool attack = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        this.player = GameObject.FindGameObjectWithTag("Player").transform;
        this.parent = animator.transform.parent;
        this.enemy = parent.GetComponent<Enemy>();
        if (this.attack) {
            this.attack = false;
            this.enemy.IsAttacking = false;
        }
    }

    private bool CheckIfEnemyMustTurn(Transform enemy) {
        bool flipEnemy = false;

        if ((player.position.x < enemy.position.x && facingRight) || (player.position.x > enemy.position.x && !facingRight)) {
            flipEnemy = true;
        }
        return flipEnemy;
    }

    /// <summary>
    /// Moves enemy towards the player by checking both objects position
    /// </summary>
    private void MoveEnemyTowardsPlayer(Transform enemy) {
        enemy.position = Vector2.MoveTowards(
            enemy.position,
            player.position,
            speed * Time.deltaTime
        );
    }

    /// <summary>
    /// The enemy has to always check if the player is not violating
    /// its vision length.
    /// </summary>
    /// <returns>True if in sight, false if not</returns>
    private bool LostPlayerOutOfSight(Transform enemy) {
        bool inSight = false;
        Vector2 enemyFront = enemy.GetComponent<Enemy>().FrontPoint.position;

        if (Vector2.Distance(enemyFront, player.position) > maxVisionLength) {
            inSight = true;
        }

        return inSight;
    }

    /// <summary>
    /// Flips the enemy around the y-axis
    /// </summary>
    private void FlipEnemy(Transform enemy) {
        facingRight = !facingRight;
        if (!facingRight) {
            enemy.Rotate(0, 180, 0);
        }
    }

    /// <summary>
    /// The player is chased by the enemy by checking the position of the player first.
    /// When it knows which direction the player is in terms of the player, it wikl move towards
    /// the player
    /// </summary>
    private void ChasePlayer(Transform enemy) {
        if (CheckIfEnemyMustTurn(enemy)) {
            FlipEnemy(enemy);
        }

        MoveEnemyTowardsPlayer(enemy);
    }

    /// <summary>
    /// Checks if player is in reach for attack
    /// </summary>
    /// <param name="enemy">The enemy to potentially attack the player</param>
    /// <returns>True if in reach, false if not</returns>
    private bool IsPlayerInAttackReach(Transform enemy) {
        bool inReach = false;
        Vector2 enemyFront = enemy.GetComponent<Enemy>().FrontPoint.position;

        if (Vector2.Distance(enemyFront, player.position) <= attackReach) {
            inReach = true;
        }

        return inReach;
    }

    private void AttackPlayer(Animator animator) {
        this.attack = true;
        enemy.IsAttacking = true;
        animator.SetTrigger("attack");
    }

    private void SearchForPlayer(Animator animator) {
        animator.SetBool("chase", false);
        animator.SetBool("patrol", true);
    }

    /// <summary>
    /// Updates the state by checking if the enemy can attack the player,
    /// if the player is not close enough, it will chase the player instead.
    /// It would always have to check if the player can be followed, if not
    /// it will go back to patrol state.
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (IsPlayerInAttackReach(parent)) {
            AttackPlayer(animator);
        } else {
            ChasePlayer(parent);
        }

        if (LostPlayerOutOfSight(parent)) {
            SearchForPlayer(animator);
        }
    }
}