using UnityEngine;

namespace _Game.Scripts
{
    public class ResetIsInteracting : StateMachineBehaviour
    {
        private static readonly int IsInteracting = Animator.StringToHash("IsInteracting");

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(IsInteracting, false);
        }
    }
}