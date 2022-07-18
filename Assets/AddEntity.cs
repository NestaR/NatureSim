using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AddEntity : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool sheepPicked = false;
    public bool wolfPicked = false;
    public bool grassPicked = false;
    public bool treePicked = false;
    public GameObject prefab;
    Vector3 mousePos;

    void Update()
    {//Get the current position of the mouse so the item can be placed at the right location
        mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        if ((sheepPicked || wolfPicked || grassPicked || treePicked) && Input.GetMouseButtonDown(0))
        {//If the sheep or wolf button is pressed it calls the function to place them

            placeEntity();
        }
        if (sheepPicked && Input.GetMouseButtonDown(2))
        {//If middle mouse button is pressed it cancels the option to place sheep
            deselectSheep();
        }
        else if (wolfPicked && Input.GetMouseButtonDown(2))
        {//If middle mouse button is pressed it cancels the option to place sheep
            deselectWolf();
        }
        else if (grassPicked && Input.GetMouseButtonDown(2))
        {//If middle mouse button is pressed it cancels the option to place sheep
            deselectGrass();
        }
        else if (treePicked && Input.GetMouseButtonDown(2))
        {//If middle mouse button is pressed it cancels the option to place sheep
            deselectTree();
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

    public void placeEntity()
    {
        Vector3 worldPos;
        //Returns ray going from camera through a screen point to get the location of the position clicked on the plane
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f))
        {//Places the sheep on the position pointed
            worldPos = hit.point;
            Instantiate(prefab, worldPos, Quaternion.identity);
        }
    }


    public void grabWolf()
    {//To check if the current action the user wants to do is add wolves
        wolfPicked = true;
        GameObject.Find("WolfButton").GetComponentInChildren<Text>().text = "Wolf Selected";
    }
    public void deselectWolf()
    {//Deselects the desired game object to add
        wolfPicked = false;
        GameObject.Find("WolfButton").GetComponentInChildren<Text>().text = "";
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

    public void grabTree()
    {//To check if the current action the user wants to do is add tree
        treePicked = true;
        GameObject.Find("TreeButton").GetComponentInChildren<Text>().text = "Tree Selected";
    }
    public void deselectTree()
    {//Deselects the desired game object to add
        treePicked = false;
        GameObject.Find("TreeButton").GetComponentInChildren<Text>().text = "";
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {//Display a tooltip when the user hovers over the button
        var parentName = transform.name;
        string str1 = parentName;
        string str2 = "Button";

        string result = str1.Replace(str2, "");

        Tooltip.ShowTooltip_Static("Add " + result);
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {//Removes the tooltip when the mouse leaves the button
        Tooltip.HideTooltip_Static();
    }
}
