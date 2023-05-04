using Tools;
using UnityEngine;
using LevelDS;

namespace Player.Controllers
{
    public class HangController
    {
        private const float HangingDistancePercentageToBlock = 1.5f;

        private readonly Inputs _inputs;
        private readonly Transform _transform;
        private readonly PlayerState _playerState;
        private readonly Rigidbody _rigidbody;

        private MultiMoveLerp _multiMoveLerp;

        public HangController(Transform transform, PlayerState playerState, Inputs inputs, Rigidbody rigidbody)
        {
            _transform = transform;
            _inputs = inputs;
            _playerState = playerState;
            _rigidbody = rigidbody;
        }

        // Entry point
        public void Hang()
        {
            if (_multiMoveLerp != null && _playerState.IsPerformingHangingAction())
            {
                if (_multiMoveLerp.IsCompleted())
                {
                    StopHangAction();
                    _multiMoveLerp = null;
                }
                else
                {
                    _transform.position = _multiMoveLerp.Lerp();
                }
            }
            else if (_playerState.CanDropOnBorder())
            {
                // TODO: check on state to switch between 3 functions
                setupDropOnBorder();
                _rigidbody.useGravity = false;
            }
            else if (_playerState.IsHangingOnBorder() && _inputs.Horizontal()) // sliding left / right
            {
                
            }
        }
        
         
        private void setupDropOnBorder()
        {
            Vector3 playerPos = _transform.position;
            
            Vector3 blockEdge = playerPos + _playerState.GetDirection() * ((Level.BlockScale / 2) * HangingDistancePercentageToBlock);
            Vector3 hangingPos = blockEdge;
            hangingPos.y -= Level.BlockScale / 1.5f;
                
            Debug.Log("Player initial pos" + playerPos);
            Debug.Log("Block edge: " + blockEdge);
            Debug.Log("Hanging pos: " + hangingPos);

            _playerState.DropOnBorder();
            _multiMoveLerp = new MultiMoveLerp(
                new[] { 1f, 2f },
                new[]
                {
                    playerPos,
                    blockEdge,
                    hangingPos
                });
        }
        
        private void StopHangAction()
        {
            if (_playerState.IsDroppingOnBorder()) // ended dropping
            {
                _playerState.StopDroppingOnBorder();
                _playerState.HangOnBorder();
            }
            else if (_playerState.IsGettingUpFromBorder()) // ended getting up
            {
                _playerState.StopGettingUpFromBorder();
                _playerState.StopHanging();
            }
            // if its hanging we don't do anything
        }
    }
}