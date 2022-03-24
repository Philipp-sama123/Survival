using System;
using UnityEngine;

namespace _Game.Scripts
{
    public class AnimatorHandler : MonoBehaviour
    {
        public Animator animator;
        private InputHandler _inputHandler;
        private PlayerLocomotion _playerLocomotion;
        public bool canRotate;

        private static int Vertical;
        private static int Horizontal;
        private static int IsInteracting;


        public void Initialize()
        {
            animator = GetComponent<Animator>();
            _inputHandler = GetComponentInParent<InputHandler>();
            _playerLocomotion = GetComponentInParent<PlayerLocomotion>();

            Vertical = Animator.StringToHash("Vertical");
            Horizontal = Animator.StringToHash("Horizontal");
            IsInteracting = Animator.StringToHash("IsInteracting");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
        {
            float snappedHorizontal;
            float snappedVertical;

            #region SnappedHorizontal

            if (horizontalMovement > 0f && horizontalMovement < 0.55f)
            {
                snappedHorizontal = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                snappedHorizontal = 1f;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                snappedHorizontal = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                snappedHorizontal = -1f;
            }
            else
            {
                snappedHorizontal = 0;
            }

            #endregion

            #region SnappedVertical

            if (verticalMovement > 0f && verticalMovement < 0.55f)
            {
                snappedVertical = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                snappedVertical = 1f;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                snappedVertical = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                snappedVertical = -1f;
            }
            else
            {
                snappedVertical = 0;
            }

            #endregion

            animator.SetFloat(Vertical, snappedVertical, 0.1f, Time.deltaTime);
            animator.SetFloat(Horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        }

        public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("IsInteracting", isInteracting);
            animator.CrossFade(targetAnimation, 0.2f);
        }

        private void OnAnimatorMove()
        {
            if (_inputHandler.isInteracting == false)
                return;

            float delta = Time.deltaTime;
            _playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            _playerLocomotion.rigidbody.velocity = velocity;
        }

        public void ActivateRotation()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false;
        }
    }
}