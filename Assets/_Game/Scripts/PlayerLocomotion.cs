using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts
{
    [RequireComponent(typeof(Rigidbody), typeof(InputHandler))]
    public class PlayerLocomotion : MonoBehaviour
    {
        private InputHandler _inputHandler;
        private AnimatorHandler _animatorHandler;
        private Transform _mainCameraTransform;
        private new Rigidbody _rigidbody;

        [HideInInspector] public Transform myTransform;

        public GameObject normalCamera;

        [Header("Stats")] [SerializeField] private float movementSpeed = 2.0f;
        [SerializeField] private float rotationSpeed = 1.0f;
        [SerializeField] private float gravityValue = -9.81f;
        [SerializeField] private float jumpHeight = -9.81f;

        private CharacterController _characterController;
        private Vector3 _playerVelocity;
        private bool _groundedPlayer;

        private Vector3 _moveDirection;

        private Vector3 _normalVector;
        private Vector3 _targetPosition;

        private static int IsInteracting;

        private void Start()
        {
            _inputHandler = GetComponent<InputHandler>();
            _characterController = GetComponent<CharacterController>(); // Todo: replace with Rigidbody
            _animatorHandler = GetComponentInChildren<AnimatorHandler>();
            _rigidbody = GetComponent<Rigidbody>();
            IsInteracting = Animator.StringToHash("IsInteracting");

            _animatorHandler.Initialize();

            myTransform = transform;
            if (Camera.main != null) _mainCameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            float delta = Time.deltaTime;
            _inputHandler.TickInput(delta);
            HandleMovement(delta);
            HandleRollingAndSprinting(delta);
        }


        public void HandleJumping()
        {
            _playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        #region Movement

        private void HandleMovement(float delta)
        {
            _moveDirection = _mainCameraTransform.forward * _inputHandler.vertical;
            _moveDirection += _mainCameraTransform.right * _inputHandler.horizontal;
            _moveDirection.Normalize();
            _moveDirection.y = 0;
            float speed = movementSpeed;
            _moveDirection *= speed;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, _normalVector);
            _rigidbody.velocity = projectedVelocity;
            _inputHandler.TickInput(delta);

            if (_animatorHandler.canRotate)
            {
                HandleRotation(delta);
            }

            _animatorHandler.UpdateAnimatorValues(_inputHandler.vertical, _inputHandler.horizontal);
        }

        private void HandleRotation(float delta)
        {
            float moveOverride = _inputHandler.moveAmount;
            Vector3 targetDirection = _mainCameraTransform.forward * _inputHandler.vertical;
            targetDirection += _mainCameraTransform.right * _inputHandler.horizontal;

            targetDirection.Normalize();
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero) targetDirection = myTransform.forward;
            float rs = rotationSpeed;
            Quaternion tr = Quaternion.LookRotation(targetDirection);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

            myTransform.rotation = targetRotation;
        }

        private void HandleRollingAndSprinting(float delta)
        {
            if (_animatorHandler.animator.GetBool(IsInteracting)) return;
            if (_inputHandler.rollFlag)
            {
                _moveDirection = _mainCameraTransform.forward * _inputHandler.vertical;
                _moveDirection += _mainCameraTransform.right * _inputHandler.horizontal;

                if (_inputHandler.vertical > 0)
                {
                    _animatorHandler.PlayTargetAnimation("DodgeForward", true);
                    _moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(_moveDirection);
                    myTransform.rotation = rollRotation;
                }
                else
                {
                    _animatorHandler.PlayTargetAnimation("DodgeBackward", true);
                }
            }
        }

        #endregion
    }
}