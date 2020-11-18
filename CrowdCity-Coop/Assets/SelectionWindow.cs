public class SelectionWindow : Window
{
    public void OnBackPress()
    {
        Close();
        UIManager.GetWindow<MainMenuWindow>().Open();
    }

    public void OnModePress(int amount)
    {
        Close();
        PlayerSetupWindow setup = UIManager.GetWindow<PlayerSetupWindow>();
        setup.SetPlayers(amount);
        setup.Open();
    }
}