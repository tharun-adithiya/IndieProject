using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float Health=100;
    public float currentHealth;
    public int pellet=5;
    public float pelletHeal=10;
    public healthController healthcontroller;
    private CharcterDamage characterDamage;
    void Start()
    {
        currentHealth = Health;
        characterDamage=GetComponent<CharcterDamage>();
        healthcontroller.SetMaxHealth(Health);
        
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)&&currentHealth<100)
        {
            Debug.Log("r is pressed");
            while (pellet >= 0)
            {
                currentHealth += pelletHeal;
                pellet--;
            }

        }
        if (characterDamage.isDamage())
        {

            currentHealth = characterDamage.setDamage();
            healthcontroller.setHealth(currentHealth);
            Debug.Log(currentHealth+"is reduced");
            characterDamage.isHurt=false;
        }
            
        
    }
}
