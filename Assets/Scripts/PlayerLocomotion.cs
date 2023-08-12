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
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        _moveDirection = _cameraObject.forward * _inputActions.VerticalInput;
        _moveDirection = _moveDirection + _cameraObject.right * _inputActions.HorizontalInput;
        _moveDirection.Normalize();
        _moveDirection.y = 0;
        _moveDirection = _moveDirection * movementSpeed;

        Vector3 movementVelocity = _moveDirection;
        _playerRigidbody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = _cameraObject.forward * _inputActions.VerticalInput;
        targetDirection = targetDirection + _cameraObject.right * _inputActions.HorizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }
      
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }
}