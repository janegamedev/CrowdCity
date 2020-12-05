using System.Collections.Generic;
using System.Linq;
using Scriptables;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerSetupWindow : Window
{
    /*public Transform parent;*/
    public List<PlayerInfo> playerInfos;
    public PlayerInputManager inputManager;

    public GameSettings gameSettings;
    private PlayerInfo LastActive => playerInfos.FirstOrDefault(x => x.gameObject.activeSelf);
    private int PlayersActive => playerInfos.Count(x => x.gameObject.activeSelf);
    private int _playersReady = 0;
    
    private int PlayersCount()
    {
        PlayerInfo info = LastActive;
        return info == null ? -1 : playerInfos.IndexOf(info);
    } 

    private void Start()
    {
        gameSettings.InitData();
    }

    private void ResetInfos()
    {
        _playersReady = 0;
        
        foreach (PlayerInfo info in playerInfos)
        {
            info.Clear();
            info.gameObject.SetActive(false);
        }

        gameSettings.players.Clear();
    }

    public void OnBackPress()
    {
        ResetInfos();
        UIManager.GetWindow<MainMenuWindow>().Open();
        Close();
    }
    
    public void OnPlayPress()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        PlayerInfo info = playerInfos[PlayersCount() + 1];
        info.gameObject.SetActive(true);
        
        PlayerSetting setting = new PlayerSetting();
        gameSettings.players.Add(setting);
        
        info.SetInfo(this, gameSettings, setting, player);
    }

    public void OnPlayerLeft(PlayerInput player)
    {

    }

    public void RemovePlayer(PlayerInfo player)
    {
        int index = playerInfos.IndexOf(player);

        if (index != -1)
        {
            player.gameObject.SetActive(false);
            player.transform.SetSiblingIndex(playerInfos.Count - 1);
            playerInfos.RemoveAt(index);
            playerInfos.Add(player);
            gameSettings.players.RemoveAt(index);
        }
    }

    public void OnPlayerReady()
    {
        _playersReady++;
        
        if(_playersReady == PlayersActive)
            Debug.Log("ALL player ready");
    }
    
    public void OnPlayerUnready()
    {
        _playersReady--;
        
        if(_playersReady == PlayersActive)
            Debug.Log("ALL player ready");
        
        if(_playersReady < PlayersActive)
            Debug.Log("Not all players ready");
    }
}