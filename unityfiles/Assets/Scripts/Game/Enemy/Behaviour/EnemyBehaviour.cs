using System;
using UnityEditor;
using UnityEngine;

public class EnemyBehavourChangeArgs : EventArgs {
    public EnemyBehaviour.BehaviourState NewBehaviourState { get; set; }
}

/// <summary>
/// Enemy AI logic. Controls enemies idle, patrol, chase and attack states.
/// 
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyBehaviour : MonoBehaviour {

    public enum BehaviourState {
        IDLE,
        PATROL,
        CHASE,
        ATTACK
    }

    [SerializeField]
    private bool visualizeVision = true;

    private readonly string WALK_TRIGGER_NAME = "walk";
    private readonly string CHASE_TRIGGER_NAME = "chase";
    private readonly string ATTACK_TRIGGER_NAME = "attack";

    private readonly int MAX_PATROLTIME = 5;
    private readonly int MIN_PATROLTIME = 3;

    private readonly int MAX_IDLE_TIME = 4;
    private readonly int MIN_IDLE_TIME = 2;
    private readonly float TIME_BETWEEN_ATTACKS = 1f;
    private readonly float ATTACK_DURATION = .3f;
    private BehaviourState CurrentState { get; set; }

    public float PatrolSpeed { get; set; }
    public float ChaseSpeed { get; set; }
    private float VisionLength { get; set; }
    private float PatrolTime { get; set; }
    private float IdleTime { get; set; }
    private float AttackTimer { get; set; }
    private float AttackDuration { get; set; }

    private Enemy Enemy { get; set; }
    private Transform EnemyTransform { get; set; }
    private Animator Animator { get; set; }

    private Transform target { get; set; }

    private PlayerHealthController TargetHealthController { get; set; }

    private bool isFacingRight = true;

    private bool detectedObstacle = false;

    [SerializeField]
    private float visionUpperBound = 5;

    [SerializeField]
    private float visionLowerBound = 3;

    [SerializeField]
    private float attackDistance = 1;

    [Tooltip("The point where the eye is")]
    [SerializeField]
    private Transform eye;

    [Tooltip("The point from where to detect obstacles")]
    [SerializeField]
    private Transform obstacleDetection;

    public static event EventHandler<EnemyBehavourChangeArgs> EnemyBehaviourStateChangeEvent;

    private void Awake() {
        if (this.transform.parent.TryGetComponent(out Enemy enemy)) {
            this.Enemy = enemy;
        } else {
            throw new MissingComponentException("Cant find Enemy script i parent.");
        }
        this.Animator = this.GetComponent<Animator>();
    }
    private void Start() {
        this.TargetHealthController = FindObjectOfType<PlayerHealthController>();
        this.target = TargetHealthController.transform;
        this.InitializeEnemyBehaviour();
    }

    private void InitializeEnemyBehaviour() {

        var type = Enemy.EnemyType;
        this.PatrolSpeed = type.PatrolSpeed;
        this.ChaseSpeed = type.ChaseSpeed;
        this.VisionLength = type.VisionLength;

        EnemyTransform = Enemy.transform;
        this.ChooseRandomStartState();
    }

    private void FixedUpdate() {
        EnemyBehavourChangeArgs args = new EnemyBehavourChangeArgs();
        args.NewBehaviourState = CurrentState;
        this.detectedObstacle = DetectObstacles();
        switch (this.CurrentState) {
            case BehaviourState.IDLE:
                TryChangeToPatrol();
                SetChaseIfCanSeeTarget();
                break;
            case BehaviourState.PATROL:
                Move(this.PatrolSpeed);
                TryChangeToIdle(this.detectedObstacle);
                SetChaseIfCanSeeTarget();
                break;
            case BehaviourState.CHASE:
                if (!IsPlayerInAttackReach()) {
                    Move(this.ChaseSpeed);
                }
                SetIdleIfLostSightOfTarget();
                SetAttackIfTargetIsInReach();
                break;
            case BehaviourState.ATTACK:
                this.Attack();
                break;
            default:
                Idle();
                break;
        }
        EnemyBehaviourStateChangeEvent?.Invoke(this, args);
    }

    private void ChooseRandomStartState() {
        if (UnityEngine.Random.Range(0, 2) == 0) {
            this.Idle();
        } else {
            this.Patrol();
        }
    }

    private void Move(float speed) {
        this.EnemyTransform.Translate(Vector3.right * Time.deltaTime * speed);
    }

    private void SetAttackIfTargetIsInReach() {
        if (IsPlayerInAttackReach() && AttackTimer < Time.time) {
            this.SetState(BehaviourState.ATTACK);
            AttackTimer = TIME_BETWEEN_ATTACKS + Time.time;
            AttackDuration = ATTACK_DURATION + Time.time;
            this.Enemy.IsAttacking = true;
        } else if (AttackDuration < Time.time && this.Enemy.IsAttacking) {
            this.Enemy.IsAttacking = false;
        }
    }

    private void SetChaseIfCanSeeTarget() {
        if (CanSeeTarget()) {
            Chase();
        }
    }

    private bool CanSeeTarget() {
        Vector2 direction = (EnemyTransform.rotation.eulerAngles.y < 90) ? Vector2.right : Vector2.left;
        float eyeHorizontal = this.eye.position.x;
        float eyeVertical = this.eye.position.y;
        float eyeReach = eyeHorizontal + (VisionLength * direction.x);
        bool isVisible = false;
        // Check if target is in proper height of the eye + bounds
        var inHeight = eyeVertical + visionUpperBound >= target.transform.position.y && eyeVertical + visionLowerBound <= target.transform.position.y;
        Vector3 debugVisionEndposition = eye.position;
        if (isFacingRight && inHeight && eyeHorizontal <= target.position.x && target.position.x < eyeReach) {
            debugVisionEndposition = target.position;
            isVisible = true;
        } else if (inHeight && eyeHorizontal >= target.position.x && target.position.x > eyeReach) {
            debugVisionEndposition = target.position;
            isVisible = true;
        }

        // For visualizing the vision
        if (this.visualizeVision) {
            Debug.DrawLine(eye.position, debugVisionEndposition, Color.magenta);
        }

        return isVisible;
    }

    /// <summary>
    /// Applies damage to the target when a reported hit occurs, and the attacking
    /// flag is raised. Resets the attacking flag when applied damage.
    /// </summary>
    public void HitTarget() {
        if (this.Enemy.IsAttacking) {
            this.TargetHealthController.ApplyDamage(this.Enemy.EnemyType.Damage);
            this.Enemy.IsAttacking = false;
        }
    }

    private void SetIdleIfLostSightOfTarget() {
        if (!CanSeeTarget()) {
            Idle();
        }
    }

    private void ChangeWalkingDirection() {
        if (this.isFacingRight) {
            this.EnemyTransform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            this.isFacingRight = false;
        } else {
            this.EnemyTransform.rotation = new Quaternion(0f, 0, 0f, 0f);
            this.isFacingRight = true;
        }
    }

    private void TryChangeToIdle(bool forceChange) {
        if (this.PatrolTime < Time.time || forceChange) {
            Idle();
        }
    }

    private void TryChangeToPatrol() {
        if (this.IdleTime < Time.time) {
            Patrol();
        }
    }

    /// <summary>
    /// Checks if player is in reach for attack
    /// </summary>
    /// <param name="enemy">The enemy to potentially attack the player</param>
    /// <returns>True if in reach, false if not</returns>
    private bool IsPlayerInAttackReach() {
        bool inReach = false;
        var eyepos = Math.Abs(eye.position.x);
        var tarpos = Math.Abs(target.position.x);
        if (Math.Abs(eyepos - tarpos) <= attackDistance) {
            inReach = true;
        }
        return inReach;
    }

    /// <summary>
    /// Casts a 2D ray from the vision point and in the direction we are facing
    /// and checks if we hit a target. if the correct target is hit,
    /// return the instance, else null.
    /// </summary>
    /// <returns>target or null if not found</returns>
    private bool DetectObstacles() {
        Vector2 rayStartPosition = obstacleDetection.position;
        // Gets the direction
        Vector2 direction = (EnemyTransform.rotation.eulerAngles.y < 90) ? Vector2.right : Vector2.left;
        RaycastHit2D hitForward = Physics2D.Raycast(rayStartPosition, direction, 3);
        if (this.visualizeVision) {
            Debug.DrawRay(rayStartPosition, 3 * direction, Color.red);
        }

        if (hitForward.collider != null) {
            if (!hitForward.transform.TryGetComponent(out Player p)) {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Prevents enemies to fall of the edge when patrolling
    /// </summary>
    private void OnTriggerExit2D(Collider2D _) {
        if (this.CurrentState == BehaviourState.PATROL) {
            Idle();
        }
    }

    private void SetState(BehaviourState state) {
        this.CurrentState = state;
    }

    private void Idle() {
        this.Animator.SetBool(this.WALK_TRIGGER_NAME, false);
        this.IdleTime = Time.time + UnityEngine.Random.Range(MIN_IDLE_TIME, MAX_IDLE_TIME);
        this.SetState(BehaviourState.IDLE);
    }

    private void Patrol() {
        this.Animator.SetBool(this.WALK_TRIGGER_NAME, true);
        this.PatrolTime = Time.time + UnityEngine.Random.Range(MIN_PATROLTIME, MAX_PATROLTIME);
        ChangeWalkingDirection();
        this.SetState(BehaviourState.PATROL);
    }

    private void Chase() {
        this.Animator.SetBool(this.WALK_TRIGGER_NAME, true);
        this.SetState(BehaviourState.CHASE);
    }

    private void Attack() {
        this.Animator.SetTrigger(this.ATTACK_TRIGGER_NAME);
        this.SetState(BehaviourState.CHASE);
    }

    /// <summary>
    /// Draws the left and right clamp in editor window, when the 
    /// object is selected.
    /// </summary>
#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        if (eye == null) { return; }
        Handles.color = Color.magenta;
        var upperBound = this.eye.position.y + this.visionUpperBound;
        var lowerBound = this.eye.position.y + this.visionLowerBound;
        var xLeft = this.eye.position.x - 2f;
        var xRight = this.eye.position.x + 2f;

        Handles.DrawLine(new Vector3(xLeft, upperBound, 0), new Vector3(xRight, upperBound, 0));
        Handles.Label(new Vector3(this.eye.position.x, upperBound, 0), "UPPER BOUND");

        Handles.DrawLine(new Vector3(xLeft, lowerBound, 0), new Vector3(xRight, lowerBound, 0));
        Handles.Label(new Vector3(this.eye.position.x, lowerBound, 0), "LOWER BOUND");

        Handles.DrawLine(new Vector3(this.eye.position.x + attackDistance, this.eye.position.y + 1, 0), new Vector3(this.eye.position.x + attackDistance, this.eye.position.y - 1, 0));
        Handles.Label(new Vector3(this.eye.position.x, this.eye.position.y, 0), "ATTACK TRIGGER");

    }
#endif
}