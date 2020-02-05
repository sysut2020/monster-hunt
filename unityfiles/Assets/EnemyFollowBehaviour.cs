using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowBehaviour : StateMachineBehaviour {

    private Transform player;

    private bool facingRight;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float attackReach = 3;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        facingRight = animator.transform.parent.GetComponent<Enemy>().FacingRight;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private bool CheckIfEnemyMustTurn(Transform enemy)
    {
        bool flipEnemy = false;

        if ((player.position.x < enemy.position.x && facingRight) || (player.position.x > enemy.position.x && !facingRight)) {
            flipEnemy = true;
        }
        return flipEnemy;
    }

    private void MoveEnemyTowardsPlayer(Transform enemy)
    {
        enemy.position = Vector2.MoveTowards(
            enemy.position,
            player.position,
            speed * Time.deltaTime);
    }

    private void FlipEnemy(Transform enemy)
    {
        facingRight = !facingRight;
        // update enemy facing right property
        Debug.Log(enemy.GetComponent<Enemy>().FacingRight);
        enemy.GetComponent<Enemy>().FacingRight = facingRight;
        Debug.Log(enemy.GetComponent<Enemy>().FacingRight);

        Vector3 enemyLocalScale = enemy.localScale;

        enemyLocalScale.x *= -1;

        enemy.localScale = enemyLocalScale;
    }

    private void ChasePlayer(Transform enemy)
    {
        if (CheckIfEnemyMustTurn(enemy))
        {
            Debug.Log("Enemy flipped");
            FlipEnemy(enemy);
        }

        MoveEnemyTowardsPlayer(enemy);
    }

    private bool IsPlayerInReach(Transform enemy) 
    {
        bool inReach = false;
        Vector2 enemyFront = enemy.GetComponent<Enemy>().FrontPoint.position;

        if (Vector2.Distance(enemyFront, player.position) <= attackReach)
        {
            inReach = true;

        }

        return inReach;
    }

    private void AttackPlayer(Animator animator)
    {
        animator.SetTrigger("attack");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        

        if (IsPlayerInReach(animator.transform.parent))
        {
            AttackPlayer(animator);
        }

        ChasePlayer(animator.transform.parent);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }
}
