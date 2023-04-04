using LevelDS;
using Tools;
using UnityEngine;

namespace Player
{
    public class JumpController 
    {
        private readonly Inputs _inputs;
        private readonly Transform _transform;
        private readonly PlayerState _playerState;
        private MoveLerpParabolic _jumpLerp;
        private const int JumpHeight = Level.BlockScale;

        public JumpController(Transform transform, Inputs inputs, PlayerState playerState, float jumpDuration)
        {
            _transform = transform;
            _inputs = inputs;
            _playerState = playerState;
            _jumpLerp = new MoveLerpParabolic(jumpDuration, JumpHeight);
        }

        public void Jump () {
            if (_inputs.Jump() && _playerState.CanJump()) {
                SetUpJump();
            }

            if (_playerState.IsJumping()) {
                
                _transform.position = _jumpLerp.Lerp();

                // Check if the jump is finished
                if (_jumpLerp.IsCompleted()) {
                    _playerState.StopJumping();
                    _jumpLerp.Reset();
                    PlayerStats.AddJump();
                }
            }
        }

        private void SetUpJump()
        {
            _playerState.StartJumping();
            _jumpLerp.StartPos = _transform.position;
            _jumpLerp.TargetPos = _playerState.SetJumpTarget(_jumpLerp.StartPos);
        }
    }
}
