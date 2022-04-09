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
        public bool isJumping;
        public bool isUsingRootMotion;

        private static readonly int IsPerformingAction = Animator.StringToHash("IsPerformingAction");
        private static readonly int IsPerformingQuickTurn = Animator.StringToHash("IsPerformingQuickTurn");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");
        private static readonly int IsUsingRootMotion = Animator.StringToHash("IsUsingRootMotion");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _inputManager = GetComponent<InputManager>();
            _playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        }

        private void Update()
        {
            _inputManager.HandleAllInputs();
        }

        private void FixedUpdate()
        {
            if (isUsingRootMotion) return; // this is very important (!) 
            _playerLocomotionManager.HandleAllMovements();
        }

        private void LateUpdate()
        {
            isPerformingAction = _animator.GetBool(IsPerformingAction);
            isPerformingQuickTurn = _animator.GetBool(IsPerformingQuickTurn);
            isUsingRootMotion = _animator.GetBool(IsUsingRootMotion);
            isJumping = _animator.GetBool(IsJumping);
        }
    }
}