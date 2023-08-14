using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputActions _inputActions;
    private CameraController _cameraController;
    private PlayerLocomotion _playerLocomotion;
    private void Awake()
    {
        _inputActions = GetComponent<InputActions>();
        _cameraController = FindObjectOfType<CameraController>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();
        
    }
    private void Update()
    {
        _inputActions.HandleAllInputs();
    }
    private void FixedUpdate()
    {
        _playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        _cameraController.HandleAllCameraMovement();
    }
}