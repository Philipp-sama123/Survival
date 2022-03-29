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
        [Header("Movement Speed")] [SerializeField]
        private float rotationSpeed = 7.5f;

        [SerializeField] private float quickTurnSpeed = 12.5f;
        private Quaternion _targetRotation;
        private Quaternion _playerRotation;

        // Falling and Jumping
        private float _gravityIntensity = -15f;
        private float _jumpHeight = 5f;
        private bool _isGrounded = true;
        private bool _isJumping = false;

        [Header("Falling")] [SerializeField] private float inAirTimer;
        [SerializeField] private float leapingVelocity = 3f;
        [SerializeField] private float fallingVelocity = 33f;
        [SerializeField] private float rayCastHeightOffset = 0.5f;
        [SerializeField] private LayerMask groundLayer;

        private void Awake()
        {
            _inputManager = GetComponent<InputManager>();
            _playerManager = GetComponent<PlayerManager>();
            _animatorManager = GetComponent<AnimatorManager>();

            playerRigidbody = GetComponent<Rigidbody>();
            if (Camera.main == null) throw new Exception("Please have an active Main-Camera in your scene!");
            _mainCameraTransform = Camera.main.transform;
        }

        public void HandleJumping()
        {
            if (_isGrounded)
            {
                // _animatorManager.animator.SetBool("IsJumping", true);
                _animatorManager.PlayAnimationWithoutRootMotion("Jump", true);

                float jumpingVelocity = Mathf.Sqrt(-2 * _gravityIntensity * _jumpHeight);
                Vector3 moveDirection = _mainCameraTransform.forward * _inputManager.verticalMovementInput;

                moveDirection += _mainCameraTransform.right * _inputManager.horizontalMovementInput;
                moveDirection.Normalize();
                moveDirection.y = 0; // prevent going up
                Vector3 playerVelocity = moveDirection;

                playerVelocity.y = jumpingVelocity; // adds jumping velocity to movement
                playerRigidbody.velocity = playerVelocity;
            }
        }

        public void HandleRotation()
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

        public void HandleFallingAndLanding()
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

                inAirTimer += Time.deltaTime;
                playerRigidbody.AddForce(transform.forward * leapingVelocity); // step of the ledge
                playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
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

                inAirTimer = 0;
                _isGrounded = true;
            }
            else
            {
                _isGrounded = false;
            }

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
    }
}