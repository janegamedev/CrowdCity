using System.Collections;
using System.Collections.Generic;
using Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionHud : MonoBehaviour
{
    public GameObject selectionGroup, readyGroup;
    public TextMeshProUGUI chooseText, currentText, timerText;

    private GameSettings _gameSettings;
    private PlayerSpawner _playerSpawner;
    private PlayerConfiguration _playerConfiguration;
    private PlayerInput _input;
    private CustomItem<string> _currentNicknameItem;
    private CustomItem<Skin> _currentSkinItem;
    private int _currentSkinIndex, _currentNicknameIndex;
    private Skin _currentSkin;
    private string _currentNickname;
    private bool _nicknameSubmitted, _skinSubmitted, _isReady;

    #region Properties

    private CustomItem<string> CurrentNicknameItem
    {
        set
        {
            if (_currentNicknameItem != null && _currentNicknameItem != value)
                _currentNicknameItem.taken = false;
            
            _currentNicknameItem = value;
            _currentNicknameItem.taken = true;
            CurrentNickname = _currentNicknameItem.value;
        }
    }
    
    private CustomItem<Skin> CurrentSkinItem
    {
        set
        {
            if (_currentSkinItem != null && _currentSkinItem != value)
                _currentSkinItem.taken = false;
            
            _currentSkinItem = value;
            _currentSkinItem.taken = true;
            CurrentSkin = _currentSkinItem.value;
        }
    }

    private Skin CurrentSkin
    {
        set
        {
            _currentSkin = value;
            _playerConfiguration.SetSkin(_currentSkin);
            currentText.text = _currentSkin.name;
        }
    }
    
    private string CurrentNickname
    {
        set
        {
            _currentNickname = value;
            _playerConfiguration.SetNickname(_currentNickname);
            currentText.text = _currentNickname;
        }
    }
    
    private bool NicknameSubmitted
    {
        set
        {
            _nicknameSubmitted = value;
            chooseText.text = _nicknameSubmitted? "CHOOSE A SKIN" : "CHOOSE A NICKNAME";
            
            SkinSubmitted = false;
        }
    }
    
    private bool SkinSubmitted
    {
        set
        {
            _skinSubmitted = value;
            Ready = _nicknameSubmitted && _skinSubmitted;
        }
    }
    
    private bool Ready
    {
        set
        {
            if(!_isReady && value)
                _playerSpawner.OnPlayerReady();
            
            _isReady = value;
            
            readyGroup.SetActive(_isReady);
            selectionGroup.SetActive(!_isReady);
        }
    }

    #endregion
    
    public void SetInfo(PlayerSpawner spawner, GameSettings gs, PlayerConfiguration s, PlayerInput input)
    {
        _playerSpawner = spawner;
        _gameSettings = gs;
        _playerConfiguration = s;
        _currentSkinIndex = 0;
        _input = input;
        _playerConfiguration.devices = _input.devices;

        CurrentSkinItem = _gameSettings.FirstAvailableColorItem;
        CurrentNicknameItem = _gameSettings.FirstAvailableNicknameItem;
        
        NicknameSubmitted = false;
    }
    
    #region Actions

    public void OnNextClick()
    {
        if(_skinSubmitted) return;
        
        if (_nicknameSubmitted)
        {
            NextSkin();
            return;
        }
        
        NextNickname();
    }
    
    public void OnPreviousClick()
    {
        if(_skinSubmitted) return;
        
        if (_nicknameSubmitted)
        {
            PreviousSkin();
            return;
        }
        
        PreviousNickname();
    }
    
    public void OnSubmit()
    {
        if(_isReady) return;

        if (_nicknameSubmitted)
        {
            SkinSubmitted = true;
            Ready = true;
            return;
        }

        NicknameSubmitted = true;
    }

    
    private void NextSkin()
    {
        _currentSkinIndex = _gameSettings.NextAvailableIndexSkin(_currentSkinIndex++);
        CurrentSkinItem = _gameSettings.skinItems[_currentSkinIndex];
    }
    
    private void PreviousSkin()
    {
        _currentSkinIndex = _gameSettings.PrevAvailableIndexColor(_currentSkinIndex--);
        CurrentSkinItem = _gameSettings.skinItems[_currentSkinIndex];
    }

    private void NextNickname()
    {
        _currentNicknameIndex = _gameSettings.NextAvailableIndexNickname(_currentNicknameIndex++);
        CurrentNicknameItem = _gameSettings.nicknameItems[_currentNicknameIndex];
    }

    private void PreviousNickname()
    {
        _currentNicknameIndex = _gameSettings.PrevAvailableIndexNickname(_currentNicknameIndex--);
        CurrentNicknameItem = _gameSettings.nicknameItems[_currentNicknameIndex];
    }
    
    #endregion

    #region Events
    
    public void Leave()
    {
        _playerSpawner.RemovePlayer(this);
    }

    public void OnTimerTrigger(bool active)
    {
        timerText.gameObject.SetActive(active);
    }
    
    public void OnTimerValueChanged(int value)
    {
        timerText.text = value.ToString();
    }

    public void OnLeave()
    {
        if (_isReady)
            _playerSpawner.OnPlayerUnready();
        
        _currentSkinItem.taken = false;
        _currentNicknameItem.taken = false;
    }

    #endregion


   
}
