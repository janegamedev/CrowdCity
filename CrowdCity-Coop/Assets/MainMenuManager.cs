using System;
using System.Collections;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        UIManager.GetWindow<MainMenuWindow>().Open();
    }
}