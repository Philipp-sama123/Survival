using System;
using UnityEngine;

namespace _Game.Scripts
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        private PlayerControls _inputActions;
        private PlayerLocomotion _playerLocomotion;
        private Animator _animator;

        private Vector2 _movementInput;
        private Vector2 _cameraInput;

        private bool _jumpInput;
        
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");

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
            }

            _inputActions.Enable();
        }

        public void Awake()
        {
            _playerLocomotion = GetComponent<PlayerLocomotion>();
            _animator = GetComponent<Animator>();
        }

        public void Update()
        {
            HandleAllInputs();
        }

        public void HandleAllInputs()
        {
            HandleMovementInput();
            // HandleSprintingInput();
            HandleJumpingInput();
            // HandleSlideInput();

            //HandleAttackInput();
            //HandleDefenseInput();
            //HandleCrouchInput();

            //HandleActionInput(); 
        }


        public void OnDisable()
        {
            _inputActions.Disable();
        }


        private void HandleMovementInput()
        {
            horizontal = _movementInput.x;
            vertical = _movementInput.y;

            Debug.Log(_movementInput);

            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = _cameraInput.x;
            mouseY = _cameraInput.y;
            _playerLocomotion.HandleMovement();
            //animatorManager.UpdateAnimatorValues(horizontalInput, moveAmount, playerLocomotion.isSprinting);

            _animator.SetFloat(Vertical, vertical, .1f, Time.deltaTime);
            _animator.SetFloat(Horizontal, horizontal, .1f, Time.deltaTime);
        }

        private void HandleJumpingInput()
        {
            if (!_jumpInput) return;
            _jumpInput = false;
            _playerLocomotion.HandleJumping();
        }
    }
}