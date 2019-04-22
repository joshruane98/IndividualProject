using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject instructionsPanel;
    public void startGame()
    {
        SceneManager.LoadScene("Preload");
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void displayInstructions()
    {
        instructionsPanel.SetActive(true);
    }

    public void closeInstructions()
    {
        instructionsPanel.SetActive(false);
    }
}
