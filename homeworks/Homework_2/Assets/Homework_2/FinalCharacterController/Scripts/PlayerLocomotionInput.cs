using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Homework2.FinalCharacterController
{
    public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
    {
        [SerializeField] private bool holdToSprint = false;

        public bool SprintToogleOn { get; private set;}
        public PlayerControls PlayerControls { get; private set; }
        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; } // Kamera hareketi için

        private void OnEnable()
        {
            if (PlayerControls == null)
            {
                PlayerControls = new PlayerControls();
            }

            PlayerControls.Enable();
            PlayerControls.PlayerLocomotionMap.Enable();
            PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
        }

        private void OnDisable()
        {
            PlayerControls.PlayerLocomotionMap.Disable();
            PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>(); // Kamera dönüşleri için input
        }

        public void OnToggleSprint(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                SprintToogleOn = holdToSprint || !SprintToogleOn;
            }
            else
            {
                SprintToogleOn = !holdToSprint && SprintToogleOn;
            }
        }
    }
}
