using System;
using UnityEngine;

namespace _Game.Scripts
{
    public class PlayerCamera : MonoBehaviour
    {
        private InputManager _inputManager;

        public Transform cameraPivot;
        public GameObject cameraObject;
        public GameObject player;

        private Vector3 _cameraFollowVelocity = Vector3.zero;
        private Vector3 _targetPosition;
        private Vector3 _cameraRotation;
        private Quaternion _targetRotation;

        [Header("Camera Speeds")] public float cameraSmoothTime = 0.2f;

        float _lookAmountVertical;
        float _lookAmountHorizontal;
        private float _maximumPivotAngle = 15f;
        private float _minimumPivotAngle = -15f;


        private void Awake()
        {
            _inputManager = FindObjectOfType<InputManager>();
        }

        public void HandleAllCameraMovement()
        {
            FollowPlayer();
            RotateCamera();
        }

        private void FollowPlayer()
        {
            _targetPosition = Vector3.SmoothDamp(
                transform.position,
                player.transform.position,
                ref _cameraFollowVelocity,
                cameraSmoothTime * Time.deltaTime
            );
            transform.position = _targetPosition;
        }

        private void RotateCamera()
        {
            _lookAmountVertical = _lookAmountVertical + (_inputManager.verticalCameraInput);
            _lookAmountHorizontal = _lookAmountHorizontal - (_inputManager.horizontalCameraInput);
            _lookAmountHorizontal = Mathf.Clamp(_lookAmountHorizontal, _minimumPivotAngle, _maximumPivotAngle);

            _cameraRotation = Vector3.zero;
            _cameraRotation.y = _lookAmountVertical;
            _targetRotation = Quaternion.Euler(_cameraRotation);
            _targetRotation = Quaternion.Slerp(transform.rotation, _targetRotation, cameraSmoothTime);
            cameraObject.transform.rotation = _targetRotation;

            _cameraRotation = Vector3.zero;
            _cameraRotation.x = _lookAmountHorizontal;
            _targetRotation = Quaternion.Euler(_cameraRotation);
            _targetRotation = Quaternion.Slerp(cameraPivot.rotation, _targetRotation, cameraSmoothTime);
            cameraPivot.localRotation = _targetRotation;
        }
    }
}