using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHealth : MonoBehaviour
{
    public bool healthShown = true;
    // Update is called once per frame
    void Update()
    {
        var objects = GameObject.FindGameObjectsWithTag("Sheep");
        var objectCount = objects.Length;
        foreach (var obj in objects)
        {//Find all sheep game objects on the scene and loop through them
            //Makes sure that all health bars are either shown or hidden
            HealthScript healthScript = obj.GetComponent<HealthScript>();
            if (healthShown)
            {//If their health bar should be shown then it will activate the slider
                healthScript.healthBarUI.SetActive(true);
            }
            else
            {//If their health bar should not be shown then it will deactivate the slider
                healthScript.healthBarUI.SetActive(false);
            }
        }
    }

    public void showHealth()
    {
        //GameObject sheepHealth = GameObject.Find("Sheep");
        var objects = GameObject.FindGameObjectsWithTag("Sheep");
        var objectCount = objects.Length;
        foreach (var obj in objects)
        {//Find all sheep game objects on the scene and loop through them        
            HealthScript healthScript = obj.GetComponent<HealthScript>();
            if (healthScript.healthBarUI.activeSelf)
            {//If the button is pressed to hide health the button text is changed and its ui is deactivated
                this.GetComponentInChildren<Text>().text = "Show Health";
                healthScript.healthBarUI.SetActive(false);
                healthShown = false;
            }
            else
            {//If the button is pressed to show health the button text is changed and its ui is activated
                this.GetComponentInChildren<Text>().text = "Hide Health";
                healthScript.healthBarUI.SetActive(true);
                healthShown = true;
            }
        }


    }
}
