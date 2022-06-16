using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButtonClick : MonoBehaviour
{
    public void ButtonResetScene()
    {
        SceneManager.LoadScene("SampleScene");

    }
    public void ButtonExitScene()
    {
        Application.Quit();
    }
}
