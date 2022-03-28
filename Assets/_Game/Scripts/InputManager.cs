using System;
using UnityEngine;

namespace _Game.Scripts
{
    public class InputManager : MonoBehaviour
    {
        private PlayerControls _inputActions;
        private PlayerManager _playerManager;
        private AnimatorManager _animatorManager;
        private Animator _animator;

        [Header("Player Movement")] public float horizontalMovementInput;
        public float verticalMovementInput;

        [Header("Camera Rotation")] public float horizontalCameraInput;
        public float verticalCameraInput;

        private Vector2 _movementInput;
        private Vector2 _cameraInput;

        [Header("Button Input")] public bool sprintInput;
        public bool jumpInput;
        public bool quickTurnInput;
        public bool dodgeInput;
        private static readonly int IsPerformingQuickTurn = Animator.StringToHash("IsPerformingQuickTurn");

        private void Awake()
        {
            _animatorManager = GetComponent<AnimatorManager>();
            _animator = GetComponent<Animator>();
            _playerManager = GetComponent<PlayerManager>();
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

                _inputActions.PlayerMovement.QuickTurn.performed += i => quickTurnInput = true;
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

            _animatorManager.PlayAnimationWithoutRootMotion(
                verticalMovementInput < 0.25f
                    ? "Jump"
                    : "Jump Move",
                true);
        }

        private void HandleQuickTurnInput()
        {
            if (_playerManager.isPerformingAction) return;
            if (!quickTurnInput) return;

            quickTurnInput = false;

            _animator.SetBool(IsPerformingQuickTurn, true);

            _animatorManager.PlayAnimationWithoutRootMotion(
                horizontalCameraInput < 0.25f
                    ? "Quick Turn Left"
                    : "Quick Turn Right",
                true);
        }

        private void HandleDodgeInput()
        {
            if (_playerManager.isPerformingAction) return;
            if (!dodgeInput) return;

            dodgeInput = false;

            _animatorManager.PlayAnimationWithoutRootMotion(
                verticalMovementInput < 0
                    ? "Dodge Backward"
                    : "Dodge Forward",
                true);
        }
    }
}