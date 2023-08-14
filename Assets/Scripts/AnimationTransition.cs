using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTransition : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        _animator.SetBool("isInteracting", isInteracting);
        _animator.CrossFade(targetAnimation, 0.2f);
        
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement)
    {
        var snappedHorizontal = SnapInput(horizontalMovement);
        var snappedVertical = SnapInput(verticalMovement);

        _animator.SetFloat("Horizontal", snappedHorizontal);
        _animator.SetFloat("Vertical", snappedVertical);
    }

    private float SnapInput(float input)
    {
        return Mathf.Abs(input) < 0.55f ? Mathf.Clamp(input, -0.5f, 0.5f) : Mathf.Sign(input);
    }
}