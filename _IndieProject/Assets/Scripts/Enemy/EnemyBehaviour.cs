//using System.Diagnostics;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform[] waypoints;
    public float walkSpeed = 5;
    private Animator anim;
    private Transform targetPos;
    private ArrowProject arrow;
    private Rigidbody2D rb;
    private int currentWaypointIndex=1;
    private PlayerMovement player;
    private bool isFacingRight = true;
    [HideInInspector]public Transform EnemyCurrentPos;
    private Transform enemeyCurrentRotation;
    
    private enum EnemyState {Walk,Shoot,Attack,Hurt};
    EnemyState CurrentState;
    bool isStateComplete;
    public float closeAttackradius;
    public LayerMask p_layer;
    public Transform attackRange;
    bool inRange;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        arrow = GetComponent<ArrowProject>();
        enemeyCurrentRotation=GetComponent<Transform>();
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        Debug.Log((0 + 1) % 2);
        Debug.Log(waypoints.Length);
        Debug.Log(currentWaypointIndex);
    }
    private void Update()
    {
        EnemyCurrentPos= transform;
        if (isStateComplete)
        {
            setState();
        }
        updateState();
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            rb.bodyType = RigidbodyType2D.Static;
            Debug.Log("Dynamic to static");
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Static to Dynamic");
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    void FixedUpdate()
    {
        MoveBetweenWayPoints();
    }

    void MoveBetweenWayPoints()
    {
        if (waypoints.Length > 0)
        {
            
            // Target waypoint
            targetPos = waypoints[currentWaypointIndex];

            // Move towards the target
            if (!arrow.isInRange())
            {
                rb.linearVelocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rb.linearVelocity.y);
            }
            else
            {
                if(player.playerCurrentPos.position.x > transform.position.x && !isFacingRight)
                {
                    RotateSprite(); // Rotate to face right
                }
                if (player.playerCurrentPos.position.x < transform.position.x && isFacingRight)
                {
                    RotateSprite(); // Rotate to face left
                }

                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Stop movement
            }
            
            // Check if close to the target waypoint
            if (Vector2.Distance(targetPos.position, transform.position) < 1f)
            {
                Debug.Log("wayPointReached");

                // Rotate sprite (flip the direction)
                transform.Rotate(0f, 180f, 0f);
                Debug.Log("SpriteRotated");

                // Reverse direction
                walkSpeed = -walkSpeed;
                if (!arrow.isInRange() && walkSpeed > 0f)
                {
                    Debug.Log("SpriteRotated");
                    transform.Rotate(0f, 180f, 0f);
                }
                rb.linearVelocity = new Vector2(walkSpeed*Time.fixedDeltaTime, rb.linearVelocity.y);
                Debug.Log("MoveInDifferentDirection");

                // Increment waypoint index
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Wrap around to avoid out-of-bounds
                Debug.Log(currentWaypointIndex);
            }
        }
    }
    void RotateSprite()
    {
        if ((player.playerCurrentPos.position.x > transform.position.x && !isFacingRight) ||
            (player.playerCurrentPos.position.x < transform.position.x && isFacingRight))
        {
            transform.Rotate(0f, 180f, 0f);
            isFacingRight = !isFacingRight;
        }
    }

    void setState()
    {
        isStateComplete = false;
        if (!arrow.isInRange())
        {
            CurrentState = EnemyState.Walk;
            playWalk();
        }
        else if (arrow.isInRange())
        {
            CurrentState = EnemyState.Shoot;
            playerShoot();
        }
        if (IsInAttackRange())
        {
            Debug.Log("InRange");
            CurrentState=EnemyState.Attack;
            
            playerAttack();
        }
    }
    void playWalk()
    {
        anim.Play("EnemyWalk");
    }
    void playerShoot()
    {
        anim.Play("EnemyShoots");
    }
    void playerAttack()
    {
        anim.Play("EnemyAttack");
        inRange = false;
    }

    void stateStatus()
    {
        if (!arrow.isInRange())
        { 
            isStateComplete = true;
        }
        if (!IsInAttackRange())
        {
            isStateComplete = true;
        }
    }
    void updateState()
    {
        switch (CurrentState)
        {
            case EnemyState.Walk:
                stateStatus();
                break;
            case EnemyState.Shoot:
                stateStatus();
                break;
            case EnemyState.Attack:
                stateStatus();
                break;

        }
    }

    public bool IsInAttackRange()
    {
       inRange= Physics2D.OverlapCircle(attackRange.position,closeAttackradius,p_layer);
       return inRange;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(waypoints[0].position, waypoints[1].position);
        Gizmos.DrawWireSphere(attackRange.position,closeAttackradius);
    }
}
