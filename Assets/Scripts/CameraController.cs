using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private InputActions _inputActions;
    private Camera _mainCamera;

    public LayerMask collisionLayers;
    public Transform cameraPivot;

    private Transform _targetTransform;
    private Transform _cameraTransform;
    private float _defaultPosition;
    private Vector3 _cameraFollowVelocity = Vector3.zero;
    private Vector3 _cameraVectorPosition;
    
    private const float CameraCollisionOffset = 0.2f;
    private const float MinCollisionOffset = 0.2f;
    private const float CameraCollisionRadius = 2;
    private const float CameraFollowSpeed = 0.2f;
    private const float CameraLookSpeed = 0.5f;
    private const float CameraPivotSpeed = 0.5f;

    private float _lookAngle;
    private float _pivotAngle;
    private const float MinPivotAngle = -35;
    private const float MaxPivotAngle = 35;

    private void Awake()
    {
        _inputActions = FindObjectOfType<InputActions>();
        _targetTransform = FindObjectOfType<PlayerController>().transform;
        _mainCamera = Camera.main; 
        if (_mainCamera != null)
        {
            _cameraTransform = _mainCamera.transform;
            _defaultPosition = _cameraTransform.localPosition.z;
        }
        else
        {
            Debug.LogError("Main camera not found!");
        }
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollision();
    }

    private void FollowTarget()
    {
        var targetPosition = Vector3.SmoothDamp
            (transform.position, _targetTransform.position, ref _cameraFollowVelocity, CameraFollowSpeed);

        transform.position = targetPosition;
    }


    private void RotateCamera()
    {
        _lookAngle += _inputActions.cameraInputX * CameraLookSpeed;
        _pivotAngle -= _inputActions.cameraInputY * CameraPivotSpeed;
        _pivotAngle = Mathf.Clamp(_pivotAngle, MinPivotAngle, MaxPivotAngle);
        
        var rotation = new Vector3(0, _lookAngle, 0);
        var targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;
        
        rotation = new Vector3(_pivotAngle, 0, 0);
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollision()
    {
        var targetPosition = _defaultPosition;
        var direction = _cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivot.transform.position, CameraCollisionRadius, direction, out var hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            var distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition = - (distance - CameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < MinCollisionOffset)
        {
            targetPosition -= MinCollisionOffset;
        }

        _cameraVectorPosition.z = Mathf.Lerp(_cameraTransform.localPosition.z, targetPosition, 0.2f);
        _cameraTransform.localPosition = _cameraVectorPosition;
    }
}