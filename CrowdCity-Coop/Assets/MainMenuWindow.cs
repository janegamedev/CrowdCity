using UnityEngine;

public class MainMenuWindow : Window
{
    public void OnPlayPress()
    {
        Close();
        UIManager.GetWindow<SelectionWindow>().Open();
    }

    public void OnExitPress()
    {
        Application.Quit();
    }
}