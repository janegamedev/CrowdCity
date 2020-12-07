using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuWindow : Window
{
    public string gameScene;
    public void OnPlayPress()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void OnExitPress()
    {
        Application.Quit();
    }
}