using System;
using UnityEngine;

namespace _Game.Scripts
{
    [RequireComponent(typeof(InputManager))]
    public class PlayerManager : MonoBehaviour
    {
        private InputManager _inputManager;
        private PlayerLocomotionManager _playerLocomotionManager;

        private void Awake()
        {
            _inputManager = GetComponent<InputManager>();
            _playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        }

        private void Update()
        {
            _inputManager.HandleAllInputs();
        }

        private void FixedUpdate()
        {
            _playerLocomotionManager.HandleRotation();
        }
    }
}