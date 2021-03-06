using UnityEngine;

namespace _Game.Scripts
{
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        [Header("Is Performing Action Bool")] public string isPerformingAction = "IsPerformingAction";
        public bool isPerformingActionStatus = false;
        [Header("Is Performing Action Bool")] public string isPerformingQuickTurn = "IsPerformingQuickTurn";
        public bool isPerformingQuickTurnStatus = false;
        [Header("Is Jumping Action Bool")] public string isJumping = "IsJumping";
        public bool isJumpingStatus = false;
        [Header("Is Using Root Motion Bool")] public string isUsingRootMotion = "IsUsingRootMotion";
        public bool isUsingRootMotionStatus = false;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(isPerformingAction, isPerformingActionStatus);
            animator.SetBool(isPerformingQuickTurn, isPerformingQuickTurnStatus);
            animator.SetBool(isUsingRootMotion, isUsingRootMotionStatus);
            animator.SetBool(isJumping, isJumpingStatus);
        }
    }
}