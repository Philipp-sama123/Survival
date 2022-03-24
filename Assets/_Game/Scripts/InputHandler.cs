using System;
using UnityEngine;

namespace _Game.Scripts
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontalMovementInput;
        public float verticalMovementInput;
        public float moveAmount;

        public float mouseX;
        public float mouseY;

        private PlayerControls _inputActions;
        private PlayerLocomotion _playerLocomotion;

        private Vector2 _movementInput;
        private Vector2 _cameraInput;
        private bool _rollInput;
        private bool _jumpInput;

        public bool rollFlag;
        public bool isInteracting;

        public void OnEnable()
        {
            if (_inputActions == null)
            {
                _inputActions = new PlayerControls();
                _inputActions.PlayerMovement.Movement.performed +=
                    i => _movementInput = i.ReadValue<Vector2>();
                _inputActions.PlayerMovement.Camera.performed += i => _cameraInput = i.ReadValue<Vector2>();

                _inputActions.PlayerMovement.Jump.performed += i => _jumpInput = true; // set true when pressed 
                _inputActions.PlayerMovement.Jump.canceled += i => _jumpInput = false;

                _inputActions.PlayerActions.Roll.performed += i => _rollInput = false;
                _inputActions.PlayerActions.Roll.canceled += i => _rollInput = true;
            }

            _inputActions.Enable();
        }

        public void Awake()
        {
            _playerLocomotion = GetComponent<PlayerLocomotion>();
        }

        public void Update()
        {
            //  HandleAllInputs();
        }

        // public void HandleAllInputs()
        //{
        // HandleMovementInput();
        //   HandleJumpingInput();
        // HandleSlideInput();

        // HandleSprintingInput();
        //HandleAttackInput();
        //HandleDefenseInput();
        //HandleCrouchInput();

        //HandleActionInput(); 
        // }

        public void OnDisable()
        {
            _inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleDodgeInput(delta);
        }

        private void MoveInput(float delta)
        {
            horizontalMovementInput = _movementInput.x;
            verticalMovementInput = _movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalMovementInput) + Mathf.Abs(verticalMovementInput));

            mouseX = _cameraInput.x;
            mouseY = _cameraInput.y;
            // _playerLocomotion.HandleMovement();
        }

        private void HandleDodgeInput(float delta)
        {
            if (_rollInput && !isInteracting)
            {
                rollFlag = true;
            }
        }

        private void HandleJumpingInput()
        {
            if (!_jumpInput) return;
            _jumpInput = false;
            _playerLocomotion.HandleJumping();
        }
    }
}