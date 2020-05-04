using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void SettingsMenu()
    {
        SceneManager.LoadScene("Settings Menu");
    }

    public void TutorialScreen()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void WinScreen()
    {
        SceneManager.LoadScene("WinScreen");
    }

    public void LoseScene()
    {
        SceneManager.LoadScene("LoseScene");
    }
}
