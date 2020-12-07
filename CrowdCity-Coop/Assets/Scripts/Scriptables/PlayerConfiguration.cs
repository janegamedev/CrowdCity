using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Scriptables
{
    public class PlayerConfiguration
    {
        public Action onNicknameUpdated = delegate {  };
        public Action onSkinUpdated = delegate {  };
        
        public string nickname;
        public Skin skin;
        public string mapScheme;
        public ReadOnlyArray<InputDevice> devices;
        public PlayerInput playerInput;
        public int playerIndex;

        public void SetNickname(string value)
        {
            nickname = value;
            onNicknameUpdated?.Invoke();
        }

        public void SetSkin(Skin value)
        {
            skin = value;
            onSkinUpdated?.Invoke();
        }
    }
}