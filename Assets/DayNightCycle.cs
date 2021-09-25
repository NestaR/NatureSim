using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DayNightCycle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Material dayMaterial;
    public Material nightMaterial;
    // Start is called before the first frame update
    public bool dayTime;
    void Start()
    {//Set time to day at the start
        dayTime = true;
    }
    public void setTime()
    {
        if(dayTime)
        {//Set time to night
            dayTime = false;
            RenderSettings.skybox = nightMaterial;
        }
        else
        {//Set time to day
            dayTime = true;
            RenderSettings.skybox = dayMaterial;
        }     
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {//Display a tooltip when the user hovers over the button
        if (dayTime)
        {
            Tooltip.ShowTooltip_Static("Change to night");
        }
        else
        {
            Tooltip.ShowTooltip_Static("Change to day");
        }
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {//Removes the tooltip when the mouse leaves the button
        Tooltip.HideTooltip_Static();
    }
}
