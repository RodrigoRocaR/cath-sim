using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsController
{
    private const string Idle = "Idle";
    private const string Run = "Run";
    private const string Jump = "Jump";
    
    private readonly Animator _animator;
    private readonly Rigidbody _rb;
    private readonly Inputs _inputs;
    private string _currentState; // for animations states 


    public AnimationsController(Animator animator, Rigidbody rb, Inputs inputs)
    {
        _animator = animator;
        _rb = rb;
        _inputs = inputs;
    }

    public void UpdateAnimations(bool isMoving) // should be called in FixedUpdate() function
    {
        //_isFalling = !_rb.IsSleeping() && _rb.velocity.y < -0.1;
        if (!isMoving)
        {
            ChangeAnimationState(Idle);
        }
        else
        {
            ChangeAnimationState(Run);
        }
    }
    
    private void ChangeAnimationState(string newState)
    {
        if (_currentState == newState) return;
        _animator.Play(newState); // play animation
        _currentState = newState; 
    }
}
