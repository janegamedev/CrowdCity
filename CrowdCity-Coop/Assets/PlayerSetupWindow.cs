using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerSetupWindow : Window
{
    /*public Transform parent;*/
    public List<PlayerInfo> playerInfos;
    public PlayerInputManager inputManager;
    public int startDelay;
    public TextMeshProUGUI timer;

    public GameSettings gameSettings;
    private PlayerInfo LastActive => playerInfos.FirstOrDefault(x => x.gameObject.activeSelf);
    private int PlayersActive => playerInfos.Count(x => x.gameObject.activeSelf);
    private int _playersReady = 0, _timer;
    private bool _timerIsOn = false;

    private bool TimerIsOn
    {
        set
        {
            if(_timerIsOn && !value)
                StopAllCoroutines();
            
            _timerIsOn = value;

            if (_timer != startDelay)
            {
                _timer = startDelay;
                timer.text = _timer.ToString();
            }
            
            timer.gameObject.SetActive(_timerIsOn);
            
            if (_timerIsOn)
                StartCoroutine(StartTimer());
        }
    }


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

        TimerIsOn = false;
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
            player.Clear();
            gameSettings.players.RemoveAt(index);
        }
    }

    public void OnPlayerReady()
    {
        _playersReady++;
        TimerIsOn = _playersReady == PlayersActive;
    }
    
    public void OnPlayerUnready()
    {
        _playersReady--;

        TimerIsOn = false;
        TimerIsOn = _playersReady == PlayersActive;
    }

    IEnumerator StartTimer()
    {
        while (_timer > 0)
        {
            timer.text = _timer.ToString();
            _timer--;
         
            yield return new WaitForSeconds(1);
        }
        
        SceneManager.LoadScene("SampleScene");
    }
}