using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowInstruct : MonoBehaviour
{
    public bool instructShown = true;
    public Image instructions;
    public GameObject inst;
    void Start()
    {
        //inst.SetActive(true);
    }

    public void showInstruct()
    {
        if (inst.activeSelf)
        {//If the button is pressed to hide the instructions then deactivate the instructions and change the text
            GameObject.Find("ShowInstructions").GetComponentInChildren<Text>().text = "Show Instructions";
            inst.SetActive(false);
            instructShown = false;
        }
        else
        {//If the button is pressed to show the instructions then activate the instructions and change the text
            GameObject.Find("ShowInstructions").GetComponentInChildren<Text>().text = "Hide Instructions";
            inst.SetActive(true);
            instructShown = true;
        }

    }
}
