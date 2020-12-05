using System.Linq;
using Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public Image colorImage;
    public TextMeshProUGUI playerName;

    public Image nicknameBackground, colorBackground, readyImage;
    public Button[] nicknameButtons, colorButtons;
    
    private PlayerSetupWindow _setupWindow;
    private PlayerSetting _playerSetting;
    private Color _currentColor;
    private string _currentNickname;
    private bool _nicknameSubmitted, _colorSubmitted, _isReady;
    private int _currentColorIndex, _currentNicknameIndex;
    private GameSettings _gameSettings;

    private CustomItem<string> _currentNicknameItem;
    private CustomItem<Color> _currentColorItem;
    
    private PlayerInput _input;
    public PlayerSetting PlayerSetting => _playerSetting;
    public PlayerInput Input => _input;

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
    
    private CustomItem<Color> CurrentColorItem
    {
        set
        {
            if (_currentColorItem != null && _currentColorItem != value)
                _currentColorItem.taken = false;
            
            _currentColorItem = value;
            _currentColorItem.taken = true;
            CurrentColor = _currentColorItem.value;
        }
    }

    private Color CurrentColor
    {
        set
        {
            _currentColor = value;
            _playerSetting.color = _currentColor;
            colorImage.color = _currentColor;
        }
    }
    
    private string CurrentNickname
    {
        set
        {
            _currentNickname = value;
            _playerSetting.nickname = _currentNickname;
            playerName.text = _currentNickname;
        }
    }

    private bool NicknameSubmitted
    {
        set
        {
            _nicknameSubmitted = value;

            nicknameBackground.enabled = !_nicknameSubmitted;
            foreach (Button button in nicknameButtons)
            {
                button.interactable = !_nicknameSubmitted;
            }
            
            ColorSubmitted = false;
        }
    }
    
    private bool ColorSubmitted
    {
        set
        {
            _colorSubmitted = value;

            colorBackground.enabled = _nicknameSubmitted && !_colorSubmitted;
            foreach (Button button in colorButtons)
            {
                button.interactable = _nicknameSubmitted && !_colorSubmitted;
            }

            Ready = _nicknameSubmitted && _colorSubmitted;
        }
    }

    private bool Ready
    {
        set
        {
            if(!_isReady && value)
                _setupWindow.OnPlayerReady();
            
            _isReady = value;
            readyImage.enabled = _isReady;
        }
    }

    public void SetInfo(PlayerSetupWindow sw, GameSettings gs, PlayerSetting s, PlayerInput input)
    {
        _setupWindow = sw;
        _gameSettings = gs;
        _playerSetting = s;
        _currentColorIndex = 0;
        _input = input;

        NicknameSubmitted = false;

        CurrentColorItem = _gameSettings.FirstAvailableColorItem;
        CurrentNicknameItem = _gameSettings.FirstAvailableNicknameItem;
    }

    public void OnSubmit()
    {
        if(_isReady) return;

        if (_nicknameSubmitted)
        {
            ColorSubmitted = true;
            Ready = true;
            return;
        }

        NicknameSubmitted = true;
    }
    
    public void OnNextClick()
    {
        if(_colorSubmitted) return;
        
        if (_nicknameSubmitted)
        {
            NextColor();
            return;
        }
        
        NextNickname();
    }

    public void OnPreviousClick()
    {
        if(_colorSubmitted) return;
        
        if (_nicknameSubmitted)
        {
            PreviousColor();
            return;
        }
        
        PreviousNickname();
    }
    
    private void NextColor()
    {
        _currentColorIndex = _gameSettings.NextAvailableIndexColor(_currentColorIndex++);
        CurrentColorItem = _gameSettings.colorItems[_currentColorIndex];
    }
    
    private void PreviousColor()
    {
        _currentColorIndex = _gameSettings.PrevAvailableIndexColor(_currentColorIndex--);
        CurrentColorItem = _gameSettings.colorItems[_currentColorIndex];
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

    public void Leave()
    {
        _setupWindow.RemovePlayer(this);
    }

    public void Clear()
    {
        if (_isReady)
            _setupWindow.OnPlayerUnready();
        
        Ready = false;
        NicknameSubmitted = false;
        ColorSubmitted = false;
        
        _currentColorItem.taken = false;
        _currentNicknameItem.taken = false;
    }
}