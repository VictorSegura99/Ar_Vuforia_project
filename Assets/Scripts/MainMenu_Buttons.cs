using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Buttons : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("DartsScene");
    }

    public void ExitButton()
    {
        Debug.Log("Quiting... Bye :(");
        Application.Quit();
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
