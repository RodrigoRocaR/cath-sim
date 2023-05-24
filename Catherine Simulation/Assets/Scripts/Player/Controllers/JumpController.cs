using System.Threading.Tasks;
using Tools.Lerps;
using UnityEngine;

namespace Player.Controllers
{
    public class JumpController 
    {
        private readonly Inputs _inputs;
        private readonly Transform _transform;
        private readonly PlayerState _playerState;
        private MoveLerpParabolic _jumpLerp;
        private bool _isDelayActive;
        
        private const int JumpHeight = GameConstants.BlockScale;
        private const int JumpDelayMilli = 50;

        public JumpController(Transform transform, Inputs inputs, PlayerState playerState, float jumpDuration)
        {
            _transform = transform;
            _inputs = inputs;
            _playerState = playerState;
            _jumpLerp = new MoveLerpParabolic(jumpDuration, JumpHeight);
        }

        public void Jump () {
            if (_inputs.Jump() && _playerState.CanJump() && !_isDelayActive) {
                SetUpJump();
            }

            if (_playerState.IsJumping()) {
                
                _transform.position = _jumpLerp.Lerp();

                // Check if the jump is finished
                if (_jumpLerp.IsCompleted()) {
                    _playerState.StopJumping();
                    _jumpLerp.Reset();
                    PlayerStats.AddJump();
                    
                    // Set delay
                    _isDelayActive = true;
                    WaitForJumpDelay();
                }
            }
        }

        private void SetUpJump()
        {
            _playerState.StartJumping();
            _jumpLerp.Setup(_transform.position, _playerState.SetJumpTarget(_transform.position));
        }

        private void WaitForJumpDelay()
        {
            Task.Delay(JumpDelayMilli).ContinueWith(_ =>
            {
                _isDelayActive = false;
            });
        }
    }
}
