using UnityEngine;

namespace _Game.Scripts
{
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        [Header("Is Performing Action Bool")] public string isPerformingAction = "IsPerformingAction";
        public bool isPerformingActionStatus = false;
        [Header("Is Performing Action Bool")] public string isPerformingQuickTurn = "IsPerformingQuickTurn";
        public bool isPerformingQuickTurnStatus = false;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(isPerformingAction, isPerformingActionStatus);
            animator.SetBool(isPerformingQuickTurn, isPerformingQuickTurnStatus);
            Debug.Log( animator.GetBool(isPerformingQuickTurn));
        }
    }
}