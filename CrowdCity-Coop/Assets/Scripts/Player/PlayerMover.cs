using System;
using UnityEngine;

namespace Player
{
    public class PlayerMover : MonoBehaviour
    {
        public CharacterController controller;
        public float moveSpeed = 3f;

        [SerializeField] private int playerIndex = -1;

        public int PlayerIndex
        {
            get => playerIndex;
            set => playerIndex = value;
        }

        private Vector3 _moveDirection;
        private Vector2 _moveInput;
        
        public void SetInputVector(Vector2 input)
        {
            _moveInput = input;
        }

        private void Update()
        {
            _moveDirection = new Vector3(_moveInput.x, 0 , _moveInput.y);
            _moveDirection = transform.TransformDirection(_moveDirection);
            _moveDirection *= moveSpeed;

            controller.Move(_moveDirection * Time.deltaTime);
        }
    }
}