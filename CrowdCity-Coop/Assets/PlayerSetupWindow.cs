using System.Collections.Generic;
using Scriptables;
using UnityEngine;

public class PlayerSetupWindow : Window
{
    public Transform parent;
    public GameObject playerInfoPrefab;

    public GameSettings gameSettings;
    private List<PlayerInfo> _playerInfos = new List<PlayerInfo>();
    public void SetPlayers(int amount)
    {
        if(_playerInfos.Count > 0)
            ResetInfos();
        
        _playerInfos = new List<PlayerInfo>();
        gameSettings.InitColorTable();
        gameSettings.players = new PlayerSetting[amount];

        for (var i = 0; i < gameSettings.players.Length; i++)
        {
            PlayerSetting setting = new PlayerSetting();
            gameSettings.players[i] = setting;
            
            PlayerInfo info = Instantiate(playerInfoPrefab, parent).GetComponent<PlayerInfo>();
            info.SetInfo(gameSettings, setting, i);
            _playerInfos.Add(info);
        }
    }

    private void ResetInfos()
    {
        for (int i = _playerInfos.Count - 1; i >= 0; i--)
        {
            Destroy(_playerInfos[i].gameObject);
        }
    }

    public void OnBackPress()
    {
        Close();
        UIManager.GetWindow<SelectionWindow>().Open();
    }
    
    public void OnPlayPress()
    {
       Debug.Log("PLAY!");
    }
}