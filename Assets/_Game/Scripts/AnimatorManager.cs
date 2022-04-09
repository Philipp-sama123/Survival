using System;
using UnityEngine;

namespace _Game.Scripts
{
    public class AnimatorManager : MonoBehaviour
    {
        private PlayerLocomotionManager _playerLocomotionManager;
        private PlayerManager _playerManager;
        public Animator animator;

        private float _snappedHorizontal;
        private float _snappedVertical;

        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int IsPerformingAction = Animator.StringToHash("IsPerformingAction");
        private static readonly int IsUsingRootMotion = Animator.StringToHash("IsUsingRootMotion");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            _playerManager = GetComponent<PlayerManager>();
            _playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        }

        public void HandleAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            #region SnappedHorizontal

            if (horizontalMovement > -0.25f && horizontalMovement < 0.25f)
            {
                _snappedHorizontal = 0;
            }
            else if (horizontalMovement > 0.25f && horizontalMovement < 0.75f)
            {
                _snappedHorizontal = 0.5f;
            }
            else if (horizontalMovement > 0.75f)
            {
                _snappedHorizontal = 1;
            }
            else if (horizontalMovement < -0.25 && horizontalMovement > -0.75f)
            {
                _snappedHorizontal = -0.5f;
            }
            else if (horizontalMovement < -0.75f)
            {
                _snappedHorizontal = -1;
            }
            else
            {
                _snappedHorizontal = 0;
            }

            #endregion

            #region SnappedVertical

            if (verticalMovement > -0.25f && verticalMovement < 0.25f)
            {
                _snappedVertical = 0;
            }
            else if (verticalMovement > 0.25f && verticalMovement < 0.75f)
            {
                _snappedVertical = 0.5f;
            }
            else if (verticalMovement > 0.75f)
            {
                _snappedVertical = 1;
            }
            else if (verticalMovement < -0.25 && verticalMovement > -0.75f)
            {
                _snappedVertical = -0.5f;
            }
            else if (verticalMovement < -0.75f)
            {
                _snappedVertical = -1;
            }
            else
            {
                _snappedVertical = 0;
            }

            #endregion

            if (isSprinting)
            {
                _snappedVertical *= 2;
            }

            animator.SetFloat(Horizontal, _snappedHorizontal, 0.1f, Time.deltaTime);
            animator.SetFloat(Vertical, _snappedVertical, 0.1f, Time.deltaTime);
        }

        public void PlayAnimationWithoutRootMotion(string targetAnimation, bool isPerformingAction)
        {
            animator.SetBool(IsPerformingAction, isPerformingAction);
            animator.applyRootMotion = false;

            animator.CrossFade(targetAnimation, 0.2f);
        }

        public void PlayAnimationWithRootMotion(string targetAnimation, bool isPerformingAction,
            bool isUsingRootMotion = true)
        {
            animator.SetBool(IsPerformingAction, isPerformingAction);
            animator.applyRootMotion = true;
            animator.SetBool(IsUsingRootMotion, isUsingRootMotion);
            animator.CrossFade(targetAnimation, 0.2f);
        }

        private void OnAnimatorMove()
        {
            if (!_playerManager.isUsingRootMotion) return;
            Vector3 animatorDeltaPosition = animator.deltaPosition;
            animatorDeltaPosition.y = 0; // prevents going up

            Vector3 velocity = animatorDeltaPosition / Time.deltaTime;

            _playerLocomotionManager.playerRigidbody.drag = 0;
            _playerLocomotionManager.playerRigidbody.velocity = velocity;
            transform.rotation *= animator.deltaRotation;
        }
    }
}