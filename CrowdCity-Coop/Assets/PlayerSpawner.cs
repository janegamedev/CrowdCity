using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player;
using Scriptables;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerSpawner : MonoBehaviour
{
    public GameSettings settings;
    public List<Transform> spawnSpots = new List<Transform>();
    public int startDelay;
    public BoolVariable checkCollider;
    
    private List<Leader> _leaders = new List<Leader>();
    private int _playersReady = 0, _timer;
    private bool _timerIsOn = false;
    
    private Action<bool> _onTimerTriggered = delegate(bool active) {  }; 
    private Action<int> _onTimerValueChanged = delegate(int value) {  };

    private Dictionary<SelectionHud, GameObject> _playerJoined;
    private List<PlayerInput> _inputs = new List<PlayerInput>();

    #region Properties
    
    private int PlayersActive => _playerJoined.Count;

    #endregion

    private void Awake()
    {
        settings.InitData();
        checkCollider.Set(false);
        _playerJoined = new Dictionary<SelectionHud, GameObject>();
    }

    private bool TimerIsOn
    {
        set
        {
            if(_timerIsOn && !value)
                StopAllCoroutines();
            
            _timerIsOn = value;

            _onTimerTriggered.Invoke(_timerIsOn);

            if (_timerIsOn)
            {
                _timer = startDelay;
                _onTimerValueChanged.Invoke(_timer);
                StartCoroutine(StartTimer());
            }
        }
    }
    
    
    public void OnPlayerJoin(PlayerInput input)
    {
        PlayerConfiguration configuration = new PlayerConfiguration();
        settings.players.Add(configuration);

        Transform random = spawnSpots[Random.Range(0, spawnSpots.Count)];
        spawnSpots.Remove(random);

        GameObject go = Instantiate(settings.playerPrefab, random.position, random.rotation, transform);

        SelectionHud hud = input.transform.parent.GetComponentInChildren<SelectionHud>();
        PlayerMover mover = go.GetComponent<PlayerMover>();
        mover.PlayerIndex = input.playerIndex;
        
        _playerJoined.Add(hud, go);
        
        hud.SetInfo(this, settings, configuration, input);
        
        _onTimerTriggered += hud.OnTimerTrigger;
        _onTimerValueChanged += hud.OnTimerValueChanged;
        
        Leader leader = go.GetComponent<Leader>();
        leader.SetConfiguration(configuration);
        _leaders.Add(leader);
        
        _inputs.Add(input);
        TimerIsOn = false;
    }

    public void OnPlayerLeft(PlayerInput input)
    {
        _inputs.Remove(input);
    }
    
    public void RemovePlayer(SelectionHud hud)
    {
        var pair = _playerJoined[hud];
        
        if (pair != null)
        {
            hud.OnLeave();
            Destroy(pair.gameObject);
            
            _onTimerTriggered -= hud.OnTimerTrigger;
            _onTimerValueChanged -= hud.OnTimerValueChanged;
            
            Destroy(hud.transform.parent.parent.gameObject);
            _playerJoined.Remove(hud);
        }

        if (_playerJoined.Count < 1)
            SceneManager.LoadScene("Menu");
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
            yield return new WaitForSeconds(1);
            
            _timer--;
            _onTimerValueChanged.Invoke(_timer);
        }
        
        StartGame();
    }

    private void StartGame()
    {
        SpawnAI();
        checkCollider.Set(true);

        foreach (PlayerInput input in _inputs)
        {
            input.SwitchCurrentActionMap("Player");
        }
    }

    private void SpawnAI()
    {
        while (_leaders.Count < settings.maxAmountOfPlayers)
        {
            PlayerConfiguration configuration = new PlayerConfiguration();
            settings.players.Add(configuration);

            var nickname = settings.RandomNicknameTable();
            nickname.taken = true;
            configuration.SetNickname(nickname.value);
        
            var skin = settings.RandomSkinTable();
            skin.taken = true;
            configuration.SetSkin(skin.value);

            Transform random = spawnSpots[Random.Range(0, spawnSpots.Count)];
            spawnSpots.Remove(random);

            GameObject go = Instantiate(settings.aiPrefab, random.position, random.rotation, transform);
        
            Leader leader = go.GetComponent<Leader>();
            leader.SetConfiguration(configuration);
            _leaders.Add(leader);
        }
    }
}
