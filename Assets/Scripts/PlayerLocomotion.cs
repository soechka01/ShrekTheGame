using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    private InputActions _inputActions;
    private Vector3 _moveDirection;
    private Transform _cameraObject;
    private Rigidbody _playerRigidbody;

    public float movementSpeed = 7;
    public float rotationSpeed = 15;

    private void Awake()
    {
        _inputActions = GetComponent<InputActions>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
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
        Vector3 moveDirectionForward = _cameraObject.forward * _inputActions.VerticalInput;
        Vector3 moveDirectionRight = _cameraObject.right * _inputActions.HorizontalInput;
        _moveDirection = (moveDirectionForward + moveDirectionRight).normalized * movementSpeed;
        _moveDirection.y = 0;
    }

    private void ApplyMovement()
    {
        _playerRigidbody.velocity = _moveDirection;
    }

    private void CalculateRotation()
    {
        Vector3 targetDirection = CalculateTargetDirection();

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }
      
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private Vector3 CalculateTargetDirection()
    {
        Vector3 targetDirection = _cameraObject.forward * _inputActions.VerticalInput + _cameraObject.right * _inputActions.HorizontalInput;
        targetDirection.y = 0;
        targetDirection.Normalize();

        return targetDirection;
    }
}
