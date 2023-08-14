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

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement)
    {
        var snappedHorizontal = SnapInput(horizontalMovement);
        var snappedVertical = SnapInput(verticalMovement);

        _animator.SetFloat("Horizontal", snappedHorizontal);
        _animator.SetFloat("Vertical", snappedVertical);
    }

    private float SnapInput(float input)
    {
        if (Mathf.Abs(input) < 0.55f)
        {
            return Mathf.Clamp(input, -0.5f, 0.5f);
        }
        return Mathf.Sign(input);
    }
}