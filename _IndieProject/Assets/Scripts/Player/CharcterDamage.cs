using UnityEngine;

public class CharcterDamage : MonoBehaviour
{
    public hitBoxCheck hitboxCheck; // Assign in the Inspector
    public Animator anim;
    public AudioManager manager; // Manager to play SFX
    public GameObject trap;

    private Vector3 currentPosition;
    private float trapPos;
    private ArrowProjection arrowProjection;
    public float trapDamage=15;
    private PlayerHealth playerHealth;
    public float currentHealth;
     public bool isHurt=false;

    private void Awake()
    {
        //manager=GetComponent<AudioManager>();
        playerHealth = GetComponent<PlayerHealth>();
        arrowProjection =GetComponent<ArrowProjection>();
        currentHealth = playerHealth.Health;
    }


    private void Update()
    {
        currentPosition = transform.position;
        trapPos = trap.transform.position.x;

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
        if ((collision.CompareTag("Trap") && !hitboxCheck.DidHitTrap()))
        {
            
            // Player gets hurt
            Debug.Log("Player is hurt!");
            manager.playSFX(manager.hurt);
            anim.SetTrigger("IsHurt");

            currentHealth -= trapDamage;
            setDamage();
            isHurt = true;
            isDamage();

            // Push the player away from the trap
            if (trapPos > currentPosition.x)
            {
                currentPosition.x -= 0.9f;
            }
            else if (trapPos < currentPosition.x)
            {
                currentPosition.x += 0.9f;
            }

            transform.position = currentPosition;
        }
        
    }
    public float setDamage()
    {
        return currentHealth;
    }
    public bool isDamage()
    {
        return isHurt;
    }

}
