using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject menupanel;
    private bool back = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {//Open menu panel
            if (menupanel.active)
            {
                menupanel.SetActive(false);

            }
            else if (!menupanel.active)
            {
                menupanel.SetActive(true);
            }
        }
    }
}
