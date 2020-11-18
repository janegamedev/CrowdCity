using System.Linq;
using Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI playerName;

    private PlayerSetting _playerSetting;
    private Color _currentColor;
    private int _currentIndex;
    private GameSettings _gameSettings;
    private ColorBool _currentColorBool;

    private ColorBool CurrentColorBool
    {
        set
        {
            if (_currentColorBool != null && _currentColorBool != value)
                _currentColorBool.taken = false;
            _currentColorBool = value;
            _currentColorBool.taken = true;
            CurrentColor = _currentColorBool.color;
        }
    }

    private Color CurrentColor
    {
        set
        {
            _currentColor = value;
            _playerSetting.Color = _currentColor;
            image.color = _currentColor;
        }
    }

    public void SetInfo(GameSettings gs, PlayerSetting s, int index)
    {
        _gameSettings = gs;
        _playerSetting = s;
        _currentIndex = 0;
        playerName.text = "Player " + (index + 1);
        
        CurrentColorBool = _gameSettings.FirstAvailableColorBool;
    }
    
    public void NextColor()
    {
        _currentIndex = _gameSettings.NextAvailableIndex(_currentIndex++);

        CurrentColorBool = _gameSettings.ColorTable[_currentIndex];
    }
    
    public void PrevColor()
    {
        _currentIndex = _gameSettings.PrevAvailableIndex(_currentIndex--);

        CurrentColorBool = _gameSettings.ColorTable[_currentIndex];
    }
}