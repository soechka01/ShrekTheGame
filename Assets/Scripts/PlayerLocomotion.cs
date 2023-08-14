using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    private PlayerController _playerController;
    private AnimationTransition _animationTransition;
    private InputActions _inputActions;
    private Vector3 _moveDirection;
    private Transform _cameraObject;
    private Rigidbody _playerRigidbody;

    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffset = 0.5f;
    public LayerMask groundLayer;

    public bool isGrounded;

    public float movementSpeed = 7;
    public float rotationSpeed = 15;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _animationTransition = GetComponent<AnimationTransition>();
        _inputActions = GetComponent<InputActions>();
        _playerRigidbody = GetComponent<Rigidbody>();
        
        if (Camera.main != null)
        {
            _cameraObject = Camera.main.transform;
        }
        else
        {
            Debug.LogError("Camera not found!");
        }
    }
    public void HandleAllMovement()
    {
        HandleFallingAndLanding();
        if(_playerController.isInteracting)
            return;
        CalculateMovement();
        CalculateRotation();
    }

    private void CalculateMovement()
    {
        CalculateMoveDirection();
        ApplyMovement();
    }
    private void CalculateMoveDirection()
    {
        var moveDirectionForward = _cameraObject.forward * _inputActions.VerticalInput;
        var moveDirectionRight = _cameraObject.right * _inputActions.HorizontalInput;
        _moveDirection = (moveDirectionForward + moveDirectionRight).normalized * movementSpeed;
        _moveDirection.y = 0;
    }
    
    private void ApplyMovement()
    {
        _playerRigidbody.velocity = _moveDirection;
    }

    private void CalculateRotation()
    {
        var targetDirection = CalculateTargetDirection();

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }
      
        var targetRotation = Quaternion.LookRotation(targetDirection);
        var playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private Vector3 CalculateTargetDirection()
    {
        var targetDirection = _cameraObject.forward * _inputActions.VerticalInput + _cameraObject.right * _inputActions.HorizontalInput;
        targetDirection.y = 0;
        targetDirection.Normalize();

        return targetDirection;
    }

    private void HandleFallingAndLanding()
    {
        var rayCastOrigin = transform.position;
        rayCastOrigin.y += rayCastHeightOffset;

        if (!isGrounded)
        {
            if (!_playerController.isInteracting)
            {
                _animationTransition.PlayTargetAnimation("Falling Loop", true);
            }

            inAirTimer += Time.deltaTime;
            _playerRigidbody.AddForce(transform.forward * leapingVelocity);
            _playerRigidbody.AddForce(-Vector3.up * (fallingVelocity * inAirTimer));
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out _, groundLayer))
        {
            if (!isGrounded && !_playerController.isInteracting)
            {
                _animationTransition.PlayTargetAnimation("Land", true);
            }

            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}