using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController 
{
    private readonly Inputs _inputs;
    private readonly Transform _transform;
    private readonly PlayerState _playerState;
    
    // Variables for jump speed and jump height
    private float _jumpSpeed;
    private float _jumpHeight = Level.BlockScale;

    // Variables to keep track of jump progress
    private float _jumpStartTime;
    private Vector3 _jumpStartPosition;

    public JumpController(Transform transform, Inputs inputs, PlayerState playerState, float jumpSpeed)
    {
        _transform = transform;
        _inputs = inputs;
        _playerState = playerState;
        _jumpSpeed = jumpSpeed;
    }

    public void Jump () {
        if (_inputs.Jump() && _playerState.CanJump()) {
            SetUpJump();
        }

        if (_playerState.IsJumping()) {
            // Calculate the jump progress
            float jumpDuration = Time.time - _jumpStartTime;
            float jumpProgress = Mathf.Clamp01(jumpDuration * _jumpSpeed);

            // Calculate the new position based on a parabolic motion
            float yOffset = _jumpHeight * Mathf.Sin(jumpProgress * Mathf.PI);
            Vector3 newPosition = Vector3.Lerp(_jumpStartPosition, _playerState.GetTarget(), jumpProgress) + yOffset * Vector3.up;

            // Move the player to the new position
            _transform.position = newPosition;

            // Check if the jump is finished
            if (jumpProgress >= 1f) {
                _playerState.StopJumping();
                PlayerStats.AddJump();
            }
        }
    }

    private void SetUpJump()
    {
        // Set up the jump
        _playerState.StartJumping();
        _jumpStartTime = Time.time;
        _jumpStartPosition = _transform.position;
        _playerState.SetJumpTarget(_jumpStartPosition);
    }
}
