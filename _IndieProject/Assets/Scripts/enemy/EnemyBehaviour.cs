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
    private ArrowBehaviour arrow;
    private Rigidbody2D rb;
    private int currentWaypointIndex = 0;
    private PlayerMovement player;
    private bool isFacingRight = true;
    private enum EnemyState {Walk,Shoot};
    EnemyState CurrentState;
    bool isStateComplete;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        arrow = GetComponent<ArrowBehaviour>();
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (isStateComplete)
        {
            setState();
        }
        updateState();
        
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
                rb.linearVelocity = new Vector2(walkSpeed*Time.fixedDeltaTime, rb.linearVelocity.y);
                Debug.Log("MoveInDifferentDirection");

                // Increment waypoint index
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Wrap around to avoid out-of-bounds
            }
        }
    }
    void RotateSprite()
    {
        transform.Rotate(0f,180, 0f); // Rotate the parent, including child objects
        isFacingRight = !isFacingRight; // Toggle the facing direction
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
            playAttack();
        }
    }
    void playWalk()
    {
        anim.Play("EnemyWalk");
    }
    void playAttack()
    {
        anim.Play("EnemyShoots");
    }

    void stateStatus()
    {
        if (!arrow.isInRange())
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

        }
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(waypoints[0].position, waypoints[1].position);
    }
}
