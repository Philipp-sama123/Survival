using System;
using UnityEngine;

namespace _Game.Scripts
{
    [RequireComponent(typeof(InputHandler))]
    public class PlayerManager : MonoBehaviour
    {
        private InputHandler _inputHandler;
        private Animator _animator;
        private static readonly int IsInteracting = Animator.StringToHash("IsInteracting");

        private void Start()
        {
            _inputHandler = GetComponent<InputHandler>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _inputHandler.isInteracting = _animator.GetBool(IsInteracting);
            _inputHandler.rollFlag = false;
        }
    }
}