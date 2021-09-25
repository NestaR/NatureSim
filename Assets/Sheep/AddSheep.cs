using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AddSheep : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool sheepPicked = false;
    public GameObject prefab;
    Vector3 mousePos;

    void Update()
    {//Get the current position of the mouse so the item can be placed at the right location
        mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        if (sheepPicked && Input.GetMouseButtonDown(0))
        {//If the sheep button is pressed it calls the function to place sheep
            
            placeSheep();
        }
        if (Input.GetMouseButtonDown(2))
        {//If middle mouse button is pressed it cancels the option to place sheep
            deselectSheep();
        }
    }

    public void grabSheep()
    {//To check if the current action the user wants to do is add sheep
        sheepPicked = true;
        GameObject.Find("SheepButton").GetComponentInChildren<Text>().text = "Sheep Selected";
    }
    public void deselectSheep()
    {//Deselects the desired game object to add
        sheepPicked = false;
        GameObject.Find("SheepButton").GetComponentInChildren<Text>().text = "";
    }
    public void placeSheep()
    {
        Vector3 wordPos;
        //Returns ray going from camera through a screen point to get the location of the position clicked on the plane
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f))
        {//Places the sheep on the position pointed
            wordPos = hit.point;
            Instantiate(prefab, wordPos, Quaternion.identity);
        }
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {//Display a tooltip when the user hovers over the button
        Tooltip.ShowTooltip_Static("Add Sheep");
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {//Removes the tooltip when the mouse leaves the button
        Tooltip.HideTooltip_Static();
    }
}
