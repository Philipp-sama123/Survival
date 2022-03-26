using System;
using UnityEngine;

namespace _Game.Scripts
{
    public class AnimatorManager : MonoBehaviour
    {
        private Animator _animator;
        private InputManager _inputManager;
        public bool canRotate;

        private static int IsInteracting;

        /**
         * Animator values
         */
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");

        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Interacting = Animator.StringToHash("IsInteracting");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _inputManager = GetComponent<InputManager>();
        }

        public void HandleAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            float snappedHorizontal;
            float snappedVertical;

            #region SnappedHorizontal

            if (horizontalMovement > -0.25f && horizontalMovement < 0.25f)
            {
                snappedHorizontal = 0;
            }
            else if (horizontalMovement > 0.25f && horizontalMovement < 0.75f)
            {
                snappedHorizontal = 0.5f;
            }
            else if (horizontalMovement > 0.75f)
            {
                snappedHorizontal = 1;
            }
            else if (horizontalMovement < -0.25 && horizontalMovement > -0.75f)
            {
                snappedHorizontal = -0.5f;
            }
            else if (horizontalMovement < -0.75f)
            {
                snappedHorizontal = -1;
            }
            else
            {
                snappedHorizontal = 0;
            }

            #endregion

            #region SnappedVertical

            if (verticalMovement > -0.25f && verticalMovement < 0.25f)
            {
                snappedVertical = 0;
            }
            else if (verticalMovement > 0.25f && verticalMovement < 0.75f)
            {
                snappedVertical = 0.5f;
            }
            else if (verticalMovement > 0.75f)
            {
                snappedVertical = 1;
            }
            else if (verticalMovement < -0.25 && verticalMovement > -0.75f)
            {
                snappedVertical = -0.5f;
            }
            else if (verticalMovement < -0.75f)
            {
                snappedVertical = -1;
            }
            else
            {
                snappedVertical = 0;
            }

            #endregion

            if (isSprinting)
            {
                snappedVertical *= 2;
            }

            _animator.SetFloat(Horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
            _animator.SetFloat(Vertical, snappedVertical, 0.1f, Time.deltaTime);
        }

        public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
        {
            _animator.applyRootMotion = isInteracting;
            _animator.SetBool(Interacting, isInteracting);
            _animator.CrossFade(targetAnimation, 0.2f);
        }
    }
}