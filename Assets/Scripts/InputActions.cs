using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputActions : MonoBehaviour
{
    private PlayerControls _playerControls;
    private Vector2 _movementInput;
    private AnimationTransition _animationTransition;
    private float _moveAmount;

    public float VerticalInput { get; private set; }
    public float HorizontalInput { get; private set; }

    private void Awake()
    {
        _animationTransition = GetComponent<AnimationTransition>();
        InitializePlayerControls();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls?.Disable();
    }

    private void InitializePlayerControls()
    {
        _playerControls = new PlayerControls();
        _playerControls.PlayerMovement.Movement.performed += OnMovementInputPerformed;
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
        CalculateMoveAmount();
        UpdateAnimator();
    }

    private void CalculateMoveAmount()
    {
        _moveAmount = Mathf.Clamp01(Mathf.Abs(HorizontalInput) + Mathf.Abs(VerticalInput));
    }

    private void UpdateAnimator()
    {
        _animationTransition.UpdateAnimatorValues(0, _moveAmount);
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        // Добавьте вызовы других методов обработки ввода, если это необходимо.
    }
}