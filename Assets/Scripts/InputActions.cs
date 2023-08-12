using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputActions : MonoBehaviour
{
    private PlayerControls _playerControls;
    private Vector2 _movementInput;

    public float VerticalInput { get; private set; }
    public float HorizontalInput { get; private set; }

    private void OnEnable()
    {
        InitializePlayerControls();
    }

    private void OnDisable()
    {
        _playerControls?.Disable();
    }

    private void InitializePlayerControls()
    {
        if (_playerControls == null)
        {
            _playerControls = new PlayerControls();
            _playerControls.PlayerMovement.Movement.performed += OnMovementInputPerformed;
        }

        _playerControls.Enable();
    }

    private void OnMovementInputPerformed(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        VerticalInput = _movementInput.y;
        HorizontalInput = _movementInput.x;
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
    }
}