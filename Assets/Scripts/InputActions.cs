using UnityEngine;
using UnityEngine.InputSystem;

public class InputActions : MonoBehaviour
{
    private PlayerControls _playerControls;
    private Vector2 _movementInput;
    private Vector2 _cameraInput;
    private AnimationTransition _animationTransition;
    private float _movementAmount;

    public float VerticalInput { get; private set; }
    public float HorizontalInput { get; private set; }
    public float cameraInputX; 
    public float cameraInputY;

    private void Awake()
    {
        _animationTransition = GetComponent<AnimationTransition>();
        InitializePlayerControls();
    }

    private void InitializePlayerControls()
    {
        _playerControls = new PlayerControls();
        _playerControls.PlayerMovement.Movement.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
        _playerControls.PlayerMovement.Camera.performed += ctx => _cameraInput = ctx.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Update()
    {
        HandleAllInputs();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        // Добавьте вызовы других методов обработки ввода, если это необходимо.
    }

    private void HandleMovementInput()
    {
        VerticalInput = _movementInput.y;
        HorizontalInput = _movementInput.x;
        cameraInputX = _cameraInput.x; 
        cameraInputY = _cameraInput.y;
        CalculateMovementAmount();
        UpdateAnimator();
    }

    private void CalculateMovementAmount()
    {
        _movementAmount = Mathf.Clamp01(Mathf.Abs(HorizontalInput) + Mathf.Abs(VerticalInput));
    }

    private void UpdateAnimator()
    {
        _animationTransition.UpdateAnimatorValues(0, _movementAmount);
    }
}
