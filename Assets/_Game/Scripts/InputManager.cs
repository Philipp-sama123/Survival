using System;
using UnityEngine;

namespace _Game.Scripts
{
    public class InputManager : MonoBehaviour
    {
        private AnimatorManager _animatorManager;
        private PlayerControls _inputActions;

        [Header("Player Movement")] public float horizontalMovementInput;
        public float verticalMovementInput;

        [Header("Camera Rotation")] public float horizontalCameraInput;
        public float verticalCameraInput;

        private Vector2 _movementInput;
        private Vector2 _cameraInput;

        // private PlayerLocomotion _playerLocomotion;

        private bool _sprintInput;
        
        private bool _jumpInput;

        public bool rollFlag;
        public bool isInteracting;

        private void Awake()
        {
            _animatorManager = GetComponent<AnimatorManager>();
        }

        public void OnEnable()
        {
            if (_inputActions == null)
            {
                _inputActions = new PlayerControls();

                _inputActions.PlayerMovement.Movement.performed += i => _movementInput = i.ReadValue<Vector2>();
                _inputActions.PlayerMovement.Camera.performed += i => _cameraInput = i.ReadValue<Vector2>();

                _inputActions.PlayerMovement.Sprint.performed += i => _sprintInput = true;
                _inputActions.PlayerMovement.Sprint.canceled += i => _sprintInput = false;
            }

            _inputActions.Enable();
        }

        public void OnDisable()
        {
            _inputActions.Disable();
        }

        public void HandleAllInputs()
        {
            HandleMovementInput();
            HandleCameraInput();
            // HandleSprintingInput();
            // HandleSlideInput();

            //HandleAttackInput();
            //HandleDefenseInput();
            //HandleCrouchInput();

            //HandleActionInput(); 
        }

        private void HandleMovementInput()
        {
            horizontalMovementInput = _movementInput.x;
            verticalMovementInput = _movementInput.y;

            _animatorManager.HandleAnimatorValues(horizontalMovementInput, verticalMovementInput, _sprintInput);
        }

        private void HandleCameraInput()
        {
            horizontalCameraInput = _cameraInput.x;
            verticalCameraInput = _cameraInput.y;
        }
    }
}