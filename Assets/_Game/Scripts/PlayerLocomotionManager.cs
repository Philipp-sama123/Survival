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

        [Header("Player Rigidbody")] public Rigidbody playerRigidbody;

        [Header("Movement Speed")] [SerializeField]
        private float rotationSpeed = 7.5f;

        [SerializeField] private float quickTurnSpeed = 12.5f;

        [Header("Rotation Variables")] private Quaternion _targetRotation;

        [Header("Jumping Variables")] private float _gravityIntensity = 2f;
        private float _jumpHeight = 10f;

        private Quaternion _playerRotation;

        private void Awake()
        {
            _inputManager = GetComponent<InputManager>();
            _playerManager = GetComponent<PlayerManager>();
            playerRigidbody = GetComponent<Rigidbody>();

            if (Camera.main == null) throw new Exception("Please have an active Main-Camera in your scene!");
            _mainCameraTransform = Camera.main.transform;
        }


        public void HandleJumping()
        {
            // if (_inputManager.GetJump())
            // {
            //     .animator.SetBool("IsJumping", true);
            // _animatorManager.PlayTargetAnimation("Jump", false);
            // float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            // Vector3 playerVelocity = moveDirection;

            // _rigidbody.AddForce(new Vector3(0, 10f, 0));
            // TODO:  juuuuump
            ;
            // }
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
                    _mainCameraTransform.eulerAngles.y + 180,
                    _mainCameraTransform.eulerAngles.z
                );
                _targetRotation = Quaternion.Euler(0, _mainCameraTransform.eulerAngles.y, 0);
                _playerRotation =
                    Quaternion.Slerp(transform.rotation, _targetRotation, quickTurnSpeed * Time.deltaTime);
                transform.rotation = _playerRotation;
            }
        }
    }
}