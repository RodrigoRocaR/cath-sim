using UnityEngine;

namespace Player
{
    public class AnimationsController
    {
        private const string Idle = "Idle";
        private const string Run = "Run";
        private const string Jump = "Jump";
    
        private readonly Animator _animator;
        private readonly PlayerState _playerState;
        private string _currentAnimationState; // for animations states 


        public AnimationsController(Animator animator, PlayerState playerState)
        {
            _animator = animator;
            _playerState = playerState;
        }

        public void UpdateAnimations() // should be called in FixedUpdate() function
        {
            //_isFalling = !_rb.IsSleeping() && _rb.velocity.y < -0.1;
            if (_playerState.IsMoving())
            {
                ChangeAnimationState(Run);
            }
            else if (_playerState.IsJumping())
            {
                ChangeAnimationState(Jump);
            }
            else
            {
                ChangeAnimationState(Idle);
            }
        }
    
        private void ChangeAnimationState(string newState)
        {
            if (_currentAnimationState == newState) return;
            _animator.Play(newState); // play animation
            _currentAnimationState = newState; 
        }
    }
}
