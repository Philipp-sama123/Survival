using System;
using UnityEngine;

namespace _Game.Scripts
{
    [RequireComponent(typeof(InputManager))]
    public class PlayerManager : MonoBehaviour
    {
        private InputManager _inputManager;
        private PlayerLocomotionManager _playerLocomotionManager;
        private Animator _animator;

        [Header("Player Flags")] public bool isPerformingAction;
        public bool isPerformingQuickTurn;

        private static readonly int IsPerformingAction = Animator.StringToHash("IsPerformingAction");
        private static readonly int IsPerformingQuickTurn = Animator.StringToHash("IsPerformingQuickTurn");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _inputManager = GetComponent<InputManager>();
            _playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        }

        private void Update()
        {
            _inputManager.HandleAllInputs();
            isPerformingAction = _animator.GetBool(IsPerformingAction);
            isPerformingQuickTurn = _animator.GetBool(IsPerformingQuickTurn);
        }

        private void FixedUpdate()
        {
            _playerLocomotionManager.HandleRotation();
            _playerLocomotionManager.HandleJumping();
        }
    }
}