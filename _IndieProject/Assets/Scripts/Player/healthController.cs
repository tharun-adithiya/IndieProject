using UnityEngine;
using UnityEngine.UI;

public class healthController : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(float maxhealth)
    {
        slider.maxValue = maxhealth;
        slider.value = maxhealth;
    }
    public void setHealth(float health)
    {
        slider.value=health;   
    }
}
