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
        if (_inputs.Jump() && !_playerState.IsJumping && !_playerState.IsMoving) {
            SetUpJump();
            
            // Set target in front and check blocks
            _playerState.Target = _jumpStartPosition + _playerState.Direction * Level.BlockScale;
            _playerState.CheckForBlocksInFront();

            if (_playerState.IsWallInFront) // same pos as start, jump on same tile
            {
                _playerState.Target = _jumpStartPosition; 
            }
            else if (_playerState.IsBlockInFront) // next pos, 1 block up
            {
                _playerState.Target += Vector3.up * Level.BlockScale;
            }
            else if (_playerState.IsBlockBelow) // next pos, 1 block down
            {
                _playerState.Target += Vector3.down * Level.BlockScale;
            }
        }

        if (_playerState.IsJumping) {
            // Calculate the jump progress
            float jumpDuration = Time.time - _jumpStartTime;
            float jumpProgress = Mathf.Clamp01(jumpDuration * _jumpSpeed);

            // Calculate the new position based on a parabolic motion
            float yOffset = _jumpHeight * Mathf.Sin(jumpProgress * Mathf.PI);
            Vector3 newPosition = Vector3.Lerp(_jumpStartPosition, _playerState.Target, jumpProgress) + yOffset * Vector3.up;

            // Move the player to the new position
            _transform.position = newPosition;

            // Check if the jump is finished
            if (jumpProgress >= 1f) {
                _playerState.IsJumping = false;
            }
        }
    }

    private void SetUpJump()
    {
        // Set up the jump
        _playerState.IsJumping = true;
        _jumpStartTime = Time.time;
        _jumpStartPosition = _transform.position;
    }
}
