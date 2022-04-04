using System;
using UnityEngine;

namespace _Game.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerLocomotionManager : MonoBehaviour
    {
        private InputManager _inputManager;
        private Transform _mainCameraTransform;
        private PlayerManager _playerManager;
        private AnimatorManager _animatorManager;

        [Header("Player Rigidbody")] public Rigidbody playerRigidbody;

        // Rotation Variables
        [Header("Movement Variables")] [SerializeField]
        private float rotationSpeed = 7.5f;

        [SerializeField] private float runningSpeed = 5f;
        [SerializeField] private float sprintingSpeed = 7f;
        [SerializeField] private float crouchingSpeedReducer = 5f;
        [SerializeField] private float walkingSpeed = 1.5f;

        [SerializeField] private float quickTurnSpeed = 12.5f;
        [SerializeField] private float gravityIntensity = -15f;
        [SerializeField] private float jumpHeight = 5f;

        private Vector3 _moveDirection;
        private Quaternion _targetRotation;
        private Quaternion _playerRotation;

        // Movement flags
        private bool _isGrounded = true;
        private bool _isJumping = false;
        private bool _isCrouching;
        private bool _isSprinting;
        private float _inAirTimer;

        [Header("Falling")] [SerializeField] private float leapingVelocity = 10f;
        [SerializeField] private float fallingVelocity = 200f;
        [SerializeField] private float rayCastHeightOffset = 0.25f;
        [SerializeField] private LayerMask groundLayer;
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");

        private void Awake()
        {
            _inputManager = GetComponent<InputManager>();
            _playerManager = GetComponent<PlayerManager>();
            _animatorManager = GetComponent<AnimatorManager>();

            playerRigidbody = GetComponent<Rigidbody>();
            if (Camera.main == null) throw new Exception("Please have an active Main-Camera in your scene!");
            _mainCameraTransform = Camera.main.transform;
        }

        private void HandleMovement()
        {
            _moveDirection = _mainCameraTransform.forward * _inputManager.verticalMovementInput;
            _moveDirection += _mainCameraTransform.right * _inputManager.horizontalMovementInput;

            _moveDirection.Normalize();
            _moveDirection.y = 0; // prevent going up


            // if (_isSprinting)
            // {
            //     _moveDirection *= sprintingSpeed;
            // }
            // else
            // {
            //     if (_inputManager.verticalMovementInput >= 0.5f)
            //     {
            //         _moveDirection *= runningSpeed;
            //     }
            //     else
            //     {
            //         _moveDirection *= walkingSpeed;
            //     }
            // }
            //
            // if (_isCrouching)
            // {
            //     _moveDirection /= crouchingSpeedReducer;
            // }
            //
            // Vector3 movementVelocity = _moveDirection;
            // playerRigidbody.velocity = movementVelocity;
        }

        public void HandleJumping()
        {
            if (_isGrounded)
            {
                // _animatorManager.animator.SetBool("IsJumping", true);

                _animatorManager.PlayAnimationWithoutRootMotion(
                    _inputManager.verticalMovementInput < 0.25f
                        ? "Jump"
                        : "Jump Move",
                    false);

                _animatorManager.animator.SetBool(IsJumping, true);
                _animatorManager.PlayAnimationWithoutRootMotion("Jump", false);
                float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
                Vector3 playerVelocity = _moveDirection;

                playerVelocity.y = jumpingVelocity; // adds jumping velocity to movement
                playerRigidbody.velocity = playerVelocity;
                // playerRigidbody.AddForce(Vector3.up * jumpingVelocity, ForceMode.Acceleration);
            }
        }

        private void HandleRotation()
        {
            _targetRotation = Quaternion.Euler(0, _mainCameraTransform.eulerAngles.y, 0);
            _playerRotation =
                Quaternion.Slerp(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);

            if (_inputManager.verticalMovementInput != 0 || _inputManager.horizontalMovementInput != 0)
            {
                transform.rotation = _playerRotation;
            }

            if (_playerManager.isPerformingQuickTurn)
            {
                // TODO: rotate Camera for 180 degree
                // --> _targetRotation = Quaternion.Euler(0, _mainCameraTransform.eulerAngles.y, 0);
                // --> _mainCameraTransform.eulerAngles.y +180 (!)  -- just on the camera object (!) (!)

                _mainCameraTransform.eulerAngles.Set(
                    _mainCameraTransform.eulerAngles.x,
                    _mainCameraTransform.eulerAngles.y + 90,
                    _mainCameraTransform.eulerAngles.z
                );
                _targetRotation = Quaternion.Euler(0, _mainCameraTransform.eulerAngles.y, 0);
                _playerRotation =
                    Quaternion.Slerp(transform.rotation, _targetRotation, quickTurnSpeed * Time.deltaTime);
                transform.rotation = _playerRotation;
            }
        }

        private void HandleFallingAndLanding()
        {
            RaycastHit hit;
            Vector3 raycastOrigin = transform.position;
            Vector3 targetPosition = transform.position; // for the feet
            raycastOrigin.y += rayCastHeightOffset;

            if (!_isGrounded && !_isJumping)
            {
                if (!_playerManager.isPerformingAction)
                {
                    _animatorManager.PlayAnimationWithoutRootMotion("Falling", true);
                }

                _inAirTimer += Time.deltaTime;
                playerRigidbody.AddForce(transform.forward * leapingVelocity);
                playerRigidbody.AddForce(-Vector3.up * fallingVelocity * _inAirTimer, ForceMode.Acceleration);
                Debug.LogWarning(-Vector3.up * _inAirTimer * fallingVelocity);
                //  -Vector3.up     --      means it pulls you downwards 
                //  * inAirTimer    --      the longer you are in the air the quicker you fall
            }

            if (Physics.SphereCast(raycastOrigin, 0.2f, -Vector3.up, out hit, groundLayer))
            {
                if (!_isGrounded && _playerManager.isPerformingAction) // maybe !_playerManager.isPerformingAction
                {
                    _animatorManager.PlayAnimationWithoutRootMotion("Landing", true);
                }

                Vector3 raycastHitPoint = hit.point; // where the raycast hits the ground 
                targetPosition.y =
                    raycastHitPoint.y; // assign the point where the raycast hits the ground to target position

                _inAirTimer = 0;
                _isGrounded = true;
            }
            else
            {
                _isGrounded = false;
            }

            _animatorManager.animator.SetBool(IsGrounded, _isGrounded);
            // this is responsible for setting the feet over the ground when the player is grounded
            if (_isGrounded && !_isJumping)
            {
                if (_playerManager.isPerformingAction || _inputManager.verticalMovementInput > 0)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
                }
                else
                {
                    transform.position = targetPosition;
                }
            }
        }

        public void HandleAllMovements()
        {
            HandleMovement();
            HandleRotation();
            HandleFallingAndLanding();
        }
    }
}