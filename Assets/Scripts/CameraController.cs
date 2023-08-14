using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private InputActions _inputActions;
    
    public Transform targetTransform;
    public Transform cameraPivot;
    public Transform cameraTransform;
    public LayerMask collisionLayers;
    private float _defaultPosition;
    private Vector3 _cameraFollowVelocity = Vector3.zero;
    private Vector3 _cameraVectorPosition;

    public float cameraCollisionOffset = 0.2f;
    public float minCollisionOffset = 0.2f;
    public float cameraCollisionRadius = 2;
    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed = 2;
    public float cameraPivotSpeed = 2;

    public float lookAngle;
    public float pivotAngle;
    public float minPivotAngle = -35;
    public float maxPivotAngle = 35;

    private void Awake()
    {
        _inputActions = FindObjectOfType<InputActions>();
        targetTransform = FindObjectOfType<PlayerController>().transform;
        cameraTransform = Camera.main.transform;
        _defaultPosition = cameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollision();
    }

    private void FollowTarget()
    {
        Vector3 targetPostion = Vector3.SmoothDamp
            (transform.position, targetTransform.position, ref _cameraFollowVelocity, cameraFollowSpeed);

        transform.position = targetPostion;
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;
        
        lookAngle = lookAngle + (_inputActions.cameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (_inputActions.cameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);
        
        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;
        
        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollision()
    {
        float targetPosition = _defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition =- (distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minCollisionOffset)
        {
            targetPosition = targetPosition - minCollisionOffset;
        }

        _cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = _cameraVectorPosition;

    }
}