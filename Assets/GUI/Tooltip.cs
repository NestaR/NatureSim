using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private static Tooltip instance;
    [SerializeField]
    private Camera uiCamera;
    private Text tooltipText;
    private RectTransform backgroundRectTransform;

    void Start()
    {
        HideTooltip();
    }

    private void ShowTooltip(string tooltipString)
    {//Activate the tooltip and change the text to represent the button being hovered       
        tooltipText.text = tooltipString;
        float textPaddingSize = 4f;
        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize * 2f, tooltipText.preferredHeight + textPaddingSize *2f);
        backgroundRectTransform.sizeDelta = backgroundSize;
        //gameObject.SetActive(true);
        gameObject.GetComponentInChildren<Text>().enabled = true;
        gameObject.GetComponentInChildren<Image>().enabled = true;
    }
    void Update()
    {//Follow the users mouse whilst a button is being hovered
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);
        transform.localPosition = localPoint;
    }
    void Awake()
    {//On activation look for the tooltip object to be updated     
        instance = this;
        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        tooltipText = transform.Find("text").GetComponent<Text>();
    }
    private void HideTooltip()
    {//Hide the tooltip
        //gameObject.SetActive(false);
        gameObject.GetComponentInChildren<Text>().enabled = false;
        gameObject.GetComponentInChildren<Image>().enabled = false;
    }
    public static void ShowTooltip_Static(string tooltipString)
    {//Pass a string with the desired tooltip text
        instance.ShowTooltip(tooltipString);
    }
    public static void HideTooltip_Static()
    {//Call the function to hide the tooltip
        instance.HideTooltip();
    }
}
