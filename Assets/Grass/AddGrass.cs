using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AddGrass : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool grassPicked = false;
    public GameObject prefab;
    AddSheep pick;
    Vector3 mousePos;
    void Update()
    {
        //Get the current position of the mouse so the item can be placed at the right location
        mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        if (grassPicked && Input.GetMouseButtonDown(0))
        {//If the grass button is pressed it calls the function to place grass           
            placeGrass();
            
        }
        if(Input.GetMouseButtonDown(2))
        {//If middle mouse button is pressed it cancels the option to place grass
            deselectGrass();            
        }
    }
    public void grabGrass()
    {//To check if the current action the user wants to do is add grass
        grassPicked = true;
        GameObject.Find("GrassButton").GetComponentInChildren<Text>().text = "Grass Selected";
    }
    public void deselectGrass()
    {//Deselects the desired game object to add
        grassPicked = false;
        GameObject.Find("GrassButton").GetComponentInChildren<Text>().text = "";
    }
    public void placeGrass()
    {
        Vector3 worldPos;
        //Returns ray going from camera through a screen point to get the location of the position clicked on the plane
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f))
        {//Places a grass prefab at the position selected on the plane
            worldPos = hit.point;
            Instantiate(prefab, worldPos, Quaternion.identity);
        }        
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {//Display a tooltip when the user hovers over the button
        Tooltip.ShowTooltip_Static("Add Grass");
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {//Removes the tooltip when the mouse leaves the button
        Tooltip.HideTooltip_Static();
    }
}