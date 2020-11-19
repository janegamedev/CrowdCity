using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public FollowPlayer camera;
        public PlayerInput playerInput;
        private PlayerMover _mover;
        
        private void Awake()
        {
            var index = playerInput.playerIndex;
            PlayerMover[] movers = FindObjectsOfType<PlayerMover>();
            _mover = movers.FirstOrDefault(x => x.PlayerIndex == index);
            
            if(_mover == null)
                Destroy(transform.parent);
            
            camera.SetTarget(_mover.transform);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if(_mover != null)
                _mover.SetInputVector(context.ReadValue<Vector2>());
        }
    }
}