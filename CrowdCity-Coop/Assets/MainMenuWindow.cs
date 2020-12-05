using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuWindow : Window
{
    public void OnPlayPress()
    {
        Close();
        UIManager.GetWindow<PlayerSetupWindow>().Open();
    }

    public void OnExitPress()
    {
        Application.Quit();
    }
}