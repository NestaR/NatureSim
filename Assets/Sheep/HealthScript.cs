using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public float health = 100;
    public float maxHealth = 100;
    public GameObject healthBarUI;
    public Slider slider;

    void Start()
    {//Set the animals health and slider to max
        health = maxHealth;
        slider.value = CalculateHealth();
    }

    void Update()
    {//Updates the slider value to accurately represent the health
        slider.value = CalculateHealth();

        if(health < maxHealth)
        {
            //healthBarUI.SetActive(true);
        }
        if(health <= 0)
        {//If the animal has no health it destroys it
            Destroy(gameObject);
        }
        if(health > maxHealth)
        {//If the animal would be healed above its max health it is instead capped
            health = maxHealth;
        }
    }

    float CalculateHealth()
    {//Calculates the percentage of health remaining to accurately update the slider
        return health / maxHealth;
    }


}
