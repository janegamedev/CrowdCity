using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class FollowPlayer : MonoBehaviour
    {
        public Vector3 offset;
        private Transform _target;
        private PlayerInput _input;
        
        public void SetTarget(Transform t)
        {
            _target = t;
        }

        private void Update()
        {
            transform.position = _target.position + offset;
        }
    }
}