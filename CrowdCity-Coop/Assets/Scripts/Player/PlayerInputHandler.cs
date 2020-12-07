using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public FollowPlayer camera;
        public PlayerInput playerInput;
        public float selectionPressDelay;
        
        private PlayerMover _mover;
        private SelectionHud _hud;
        
        private bool _pressed = false;
        
        private void Awake()
        {
            var index = playerInput.playerIndex;
            PlayerMover[] movers = FindObjectsOfType<PlayerMover>();
            _mover = movers.FirstOrDefault(x => x.PlayerIndex == index);

            if (_mover == null)
            {
                Debug.Log("Destroy");
                Destroy(transform.parent);
                return;
            }
            
            _hud = transform.parent.GetComponentInChildren<SelectionHud>();
            
            camera.SetTarget(_mover.transform);
        }

        #region In game

        public void OnMove(InputAction.CallbackContext context)
        {
            if(_mover != null)
                _mover.SetInputVector(context.ReadValue<Vector2>());
        }

        #endregion


        #region Selection

        public void OnSubmit()
        {
            if(_pressed) return;
        
            StartCoroutine(PressDelay());
            _hud.OnSubmit();
        }

        public void OnLeave()
        {
            if(_pressed) return;
        
            StartCoroutine(PressDelay());
            _hud.Leave();
            Destroy(gameObject);
        }

        public void OnNext()
        {
            if(_pressed) return;
        
            StartCoroutine(PressDelay());
            _hud.OnNextClick();
        }

        public void OnPrevious()
        {
            if(_pressed) return;
        
            StartCoroutine(PressDelay());
            _hud.OnPreviousClick();
        }

        IEnumerator PressDelay()
        {
            _pressed = true;
        
            yield return new WaitForSeconds(selectionPressDelay);

            _pressed = false;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        #endregion
    }
}