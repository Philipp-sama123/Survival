using System;
using UnityEngine;

namespace _Game.Scripts
{
    public class InputManager : MonoBehaviour
    {
        private PlayerControls _inputActions;
        private PlayerManager _playerManager;
        private AnimatorManager _animatorManager;
        private PlayerLocomotionManager _playerLocomotionManager;
        private Animator _animator;

        [Header("Player Movement")] public float horizontalMovementInput;
        public float verticalMovementInput;

        [Header("Camera Rotation")] public float horizontalCameraInput;
        public float verticalCameraInput;

        private Vector2 _movementInput;
        private Vector2 _cameraInput;

        [Header("Button Input")] public bool sprintInput;
        public bool jumpInput;
        public bool quickTurnRightInput;
        public bool quickTurnLeftInput;
        public bool dodgeInput;
        private static readonly int IsPerformingQuickTurn = Animator.StringToHash("IsPerformingQuickTurn");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");

        private void Awake()
        {
            _playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            _animatorManager = GetComponent<AnimatorManager>();
            _playerManager = GetComponent<PlayerManager>();
            _animator = GetComponent<Animator>();
        }

        public void OnEnable()
        {
            if (_inputActions == null)
            {
                _inputActions = new PlayerControls();

                _inputActions.PlayerMovement.Movement.performed += i => _movementInput = i.ReadValue<Vector2>();
                _inputActions.PlayerMovement.Camera.performed += i => _cameraInput = i.ReadValue<Vector2>();

                _inputActions.PlayerMovement.Sprint.performed += i => sprintInput = true;
                _inputActions.PlayerMovement.Sprint.canceled += i => sprintInput = false;

                _inputActions.PlayerMovement.QuickTurnRight.performed += i => quickTurnRightInput = true;
                _inputActions.PlayerMovement.QuickTurnLeft.performed += i => quickTurnLeftInput = true;
                _inputActions.PlayerMovement.Jump.performed += i => jumpInput = true;
                _inputActions.PlayerMovement.Dodge.performed += i => dodgeInput = true;
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

            HandleQuickTurnInput();
            HandleDodgeInput();

            HandleJumpInput();
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

            _animatorManager.HandleAnimatorValues(horizontalMovementInput, verticalMovementInput, sprintInput);
        }

        private void HandleCameraInput()
        {
            horizontalCameraInput = _cameraInput.x;
            verticalCameraInput = _cameraInput.y;
        }

        private void HandleJumpInput()
        {
            if (_playerManager.isPerformingAction) return;
            if (!jumpInput) return;

            jumpInput = false;
            _playerLocomotionManager.HandleJumping();
            _animator.SetBool(IsJumping, true);
        }

        private void HandleQuickTurnInput()
        {
            if (_playerManager.isPerformingAction) return;
            if (!quickTurnRightInput && !quickTurnLeftInput) return;
            
            // todo : make this depending on the player Position 
            _animatorManager.PlayAnimationWithRootMotion(
                quickTurnRightInput
                    ? "Quick Turn Right"
                    : "Quick Turn Left",
                true);

            quickTurnRightInput = false;
            quickTurnLeftInput = false;

            _animator.SetBool(IsPerformingQuickTurn, true);
        }

        private void HandleDodgeInput()
        {
            if (_playerManager.isPerformingAction) return;
            if (!dodgeInput) return;

            dodgeInput = false;

            _animatorManager.PlayAnimationWithRootMotion(
                verticalMovementInput < 0
                    ? "Dodge Backward"
                    : "Dodge Forward",
                true);
        }
    }
}