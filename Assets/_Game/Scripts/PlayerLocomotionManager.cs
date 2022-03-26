using System;
using UnityEngine;

namespace _Game.Scripts
{
    public class PlayerLocomotionManager : MonoBehaviour
    {
        private InputManager _inputManager;
        private Transform _mainCameraTransform;

        [Header("Movement Speed")] [SerializeField]
        private float rotationSpeed = 3.5f;

        [Header("Rotation Variables")] private Quaternion _targetRotation;

        private Quaternion _playerRotation;

        private void Awake()
        {
            _inputManager = GetComponent<InputManager>();

            if (Camera.main == null) throw new Exception("Please have an active Main-Camera in your scene!");
            _mainCameraTransform = Camera.main.transform;
        }

        public void HandleRotation()
        {
            _targetRotation = Quaternion.Euler(0, _mainCameraTransform.eulerAngles.y, 0);
            _playerRotation = Quaternion.Slerp(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);

            if (_inputManager.verticalMovementInput != 0 || _inputManager.horizontalMovementInput != 0)
            {
                transform.rotation = _playerRotation;
            }
        }
    }
}