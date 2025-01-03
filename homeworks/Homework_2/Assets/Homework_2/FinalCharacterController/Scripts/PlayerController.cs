using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Homework2.FinalCharacterController
{
    [DefaultExecutionOrder(-1)]
    public class PlayerController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Camera _playerCamera;

        [Header("Base Movement")]
        public float runAcceleration = 35f;
        public float runSpeed = 4f;
        public float sprintAcceleration = 50f;
        public float sprintSpeed = 7f;
        public float drag = 20f;
        public float movingThreshold = 0.01f;

        [Header("Camera Settings")]
        public float lookSenseH = 0.1f;
        public float lookSenseV = 0.1f;
        public float lookLimitV = 89f;

        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerState _playerState;

        private Vector3 _currentVelocity;
        private Vector2 _cameraRotation = Vector2.zero;
        private Vector2 _playerTargetRotation = Vector2.zero;

        private void Awake()
        {
            if (_characterController == null)
                Debug.LogError("CharacterController is not assigned!");

            if (_playerCamera == null)
                Debug.LogError("PlayerCamera is not assigned!");

            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            if (_playerLocomotionInput == null)
                Debug.LogError("PlayerLocomotionInput component is missing!");

            _playerState = GetComponent<PlayerState>();
            if (_playerState == null)
                Debug.LogError("PlayerState component is missing!");
        }

        private void Update()
        {
            if (_characterController == null || _playerCamera == null || _playerLocomotionInput == null || _playerState == null)
                return;

            UpdateMovementState();
            HandleLateralMovement();
        }

        private void UpdateMovementState()
        {
            bool isMovementInput = _playerLocomotionInput.MovementInput != Vector2.zero;
            bool isMovingLaterally = IsMovingLaterally();
            bool isSprinting = _playerLocomotionInput.SprintToogleOn && isMovingLaterally;

            PlayerMovementState lateralState = isSprinting ? PlayerMovementState.Sprinting :
                                               isMovingLaterally || isMovementInput ? PlayerMovementState.Running : PlayerMovementState.Idling;

            _playerState.SetPlayerMovementState(lateralState);
        }

        private void HandleLateralMovement()
        {
            // Create quick references for current state
            bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;

            float lateralAcceleration = isSprinting ? sprintAcceleration : runAcceleration;
            float clampLateralMagnitude = isSprinting ? sprintSpeed : runSpeed;

            Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
            Vector3 movementDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x + cameraForwardXZ * _playerLocomotionInput.MovementInput.y;

            Vector3 movementDelta = movementDirection * lateralAcceleration * Time.deltaTime;
            Vector3 newVelocity = _currentVelocity + movementDelta;

            // Add drag to player
            Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;
            newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
            newVelocity = Vector3.ClampMagnitude(newVelocity, clampLateralMagnitude);

            // Apply gravity
            if (!_characterController.isGrounded)
            {
                newVelocity.y += Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                newVelocity.y = 0f;
            }

            // Update velocity
            _currentVelocity = newVelocity;

            // Move character
            _characterController.Move(_currentVelocity * Time.deltaTime);
        }

        private void LateUpdate()
        {
            _cameraRotation.x += lookSenseH * _playerLocomotionInput.LookInput.x;
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSenseV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);

            _playerTargetRotation.x += transform.eulerAngles.x + lookSenseH * _playerLocomotionInput.LookInput.x;
            transform.rotation = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);

            _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);
        }

        private bool IsMovingLaterally()
        {
            Vector3 lateralVelocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z);
            return lateralVelocity.magnitude > movingThreshold;
        }
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Çarpışma oldu: " + other.name); // Çarpışan nesne adını yazdır
            if (other.CompareTag("Key"))
                {
                    Debug.Log("Anahtar toplandı!");
                    GameManager.Instance.CollectKey(other.gameObject);
                }
            else
            {
                Debug.Log("Tag eşleşmiyor: " + other.tag);
            }
        }
    }
}
