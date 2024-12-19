using UnityEngine;

public class CharcterDamage : MonoBehaviour
{
    public hitBoxCheck hitboxCheck; // Assign in the Inspector
    public Animator anim;
    public AudioManager manager; // Manager to play SFX
    public GameObject trap;
    public GameObject arrow;

    private Vector3 currentPosition;
    private float trapPos;
    private float arrowPos;
    private ArrowProjection arrowProjection;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        arrowProjection =GetComponent<ArrowProjection>();
        playerMovement = GetComponent<PlayerMovement>();
        
    }


    private void Update()
    {
        currentPosition = transform.position;
        trapPos = trap.transform.position.x;
        arrowPos = arrow.transform.position.x;

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Player Attacks");
            float attackDuration = 0.5f; // Define the sword attack duration

            // Start the sword attack
            hitboxCheck.StartAttack(attackDuration);

            // Check if the sword hit a trap
            if (hitboxCheck.DidHitTrap())
            {
                Debug.Log("Sword successfully hit a trap!");
            }
            else
            {
                Debug.Log("Sword did not hit anything");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //collision=GetComponent<Collider2D>();
        if (collision.CompareTag("Trap") && !hitboxCheck.DidHitTrap())
        {
            // Player gets hurt
            Debug.Log("Player is hurt!");
            manager.playSFX(manager.hurt);
            anim.SetTrigger("IsHurt");


            if (trapPos > currentPosition.x && arrowProjection.isArrow())
            {
                currentPosition.x -= 0.9f;
            }
            else if (trapPos < currentPosition.x && arrowProjection.isArrow())
            {
                currentPosition.x += 0.9f;
            }


            transform.position = currentPosition;
        }

    }
}
