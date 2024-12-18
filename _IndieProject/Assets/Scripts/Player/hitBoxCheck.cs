using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class hitBoxCheck : MonoBehaviour
{

    private bool hitTrap; // Tracks collision with a trap
    private bool isActive; // Tracks whether the sword is active

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Trap") && isActive)|| (collision.CompareTag("MovingTraps") && isActive))
        {
            Debug.Log("Sword hit the trap!");
            hitTrap = true; // Register that the sword hit a trap
        }
        if (collision.CompareTag("MovingTraps")&&isActive)
        {
            Debug.Log("Sword hit the trap!");
            hitTrap = true; // Register that the sword hit a trap
        }
    }

    public void StartAttack(float attackDuration)
    {
        StartCoroutine(AttackCoroutine(attackDuration));
    }

    private IEnumerator AttackCoroutine(float duration)
    {
        isActive = true; // Activate the sword hitbox
        yield return new WaitForSeconds(duration); // Keep it active for the attack duration
        isActive = false; // Deactivate after the attack
        hitTrap = false; // Reset the flag
    }

    public bool DidHitTrap()
    {
        return hitTrap; // Returns true if the sword hit a trap during the active state
    }
    
    


}


