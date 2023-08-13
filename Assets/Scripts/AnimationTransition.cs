using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTransition : MonoBehaviour
{
    private Animator _animator;
    private int _horizontal;
    private int _vertical;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _horizontal = Animator.StringToHash("Horizontal");
        _vertical = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement)
    {
        float snappedHorizontal = SnapInput(horizontalMovement);
        float snappedVertical = SnapInput(verticalMovement);

        _animator.SetFloat(_horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        _animator.SetFloat(_vertical, snappedVertical, 0.1f, Time.deltaTime);
    }

    private float SnapInput(float input)
    {
        return Mathf.Abs(input) < 0.55f ? Mathf.Clamp(input, -0.5f, 0.5f) : Mathf.Sign(input);
    }

}