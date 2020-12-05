using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputHandler : MonoBehaviour
{
    public PlayerInput playerInput;
    public float pressDelay;
    
    private PlayerInfo _info;
    private bool _pressed = false;

    private void Start()
    {
        var index = playerInput.playerIndex;
        PlayerInfo[] infos = FindObjectsOfType<PlayerInfo>();
        _info = infos.FirstOrDefault(x => x.Input == playerInput);
            
        if(_info == null)
            Destroy(transform.parent);
    }

    public void OnSubmit()
    {
        if(_pressed) return;
        
        StartCoroutine(PressDelay());
        _info.OnSubmit();
    }

    public void OnLeave()
    {
        if(_pressed) return;
        
        StartCoroutine(PressDelay());
        _info.Leave();
        Destroy(gameObject);
    }

    public void NextColor()
    {
        if(_pressed) return;
        
        StartCoroutine(PressDelay());
        _info.OnNextClick();
    }

    public void PreviousColor()
    {
        if(_pressed) return;
        
        StartCoroutine(PressDelay());
        _info.OnPreviousClick();
    }

    IEnumerator PressDelay()
    {
        _pressed = true;
        
        yield return new WaitForSeconds(pressDelay);

        _pressed = false;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
