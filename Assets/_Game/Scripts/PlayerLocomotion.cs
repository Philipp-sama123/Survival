using System;
using UnityEngine;

namespace _Game.Scripts
{
    [RequireComponent(typeof(CharacterController), typeof(InputHandler))]
    public class PlayerLocomotion : MonoBehaviour
    {
        [SerializeField] private float playerSpeed = 2.0f;
        [SerializeField] private float jumpHeight = 1.0f;
        [SerializeField] private float gravityValue = -9.81f;

        private InputHandler _inputHandler;
        private CharacterController _characterController;
        private Vector3 _playerVelocity;
        private bool _groundedPlayer;

        private Transform _mainCameraTransform;

        private void Awake()
        {
            _inputHandler = GetComponent<InputHandler>();
            _characterController = GetComponent<CharacterController>();
            _mainCameraTransform = Camera.main.transform;
        }

        public void HandleJumping()
        {
            _playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }


        public void HandleMovement()
        {
            _groundedPlayer = _characterController.isGrounded;
            if (_groundedPlayer && _playerVelocity.y < 0)
            {
                _playerVelocity.y = 0f;
            }

            var move = new Vector3(_inputHandler.horizontal, 0, _inputHandler.vertical);
            //move = _mainCameraTransform.forward * move.z + _mainCameraTransform.right * move.x;
            move.y = 0;

            _characterController.Move(move * Time.deltaTime * playerSpeed);

            _playerVelocity.y += gravityValue * Time.deltaTime;
            _characterController.Move(_playerVelocity * Time.deltaTime);
        }
    }
}