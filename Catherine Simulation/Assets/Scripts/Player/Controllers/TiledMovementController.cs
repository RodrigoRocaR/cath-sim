using Tools;
using UnityEngine;

namespace Player.Controllers
{
    public class TiledMovementController
    {
        private readonly Inputs _inputs;
        private readonly Transform _transform;
        private readonly float _speed;
        private readonly PlayerState _playerState;

        public TiledMovementController(Transform transform, Inputs inputs, PlayerState playerState, float speed)
        {
            _transform = transform;
            _speed = speed;
            _inputs = inputs;
            _playerState = playerState;
        }
    
        public void Move()
        {
            if (_transform.position == _playerState.GetTarget()){ 
                _playerState.StopMoving(); // we reached the target => reset to false
                PlayerStats.AddBlocksWalked();
                _playerState.SetTarget(Vector3.positiveInfinity);
            }
        
            if (_playerState.IsJumping() || _playerState.IsFalling() || _playerState.IsPerformingHangingAction() ||
                (!_playerState.IsMoving() && (_inputs.Jump() || !_inputs.AnyInputs()))) return;

            // Movement
            if (!_playerState.IsMoving())
            {
                if (!_inputs.AnyInputs()) return;
            
                RotateAndSetTarget();
                _playerState.CheckBlocksTarget();
            
                if (!_playerState.CanMove()) return;

                _transform.position = Vector3.MoveTowards(_transform.position, _playerState.GetTarget(), Time.deltaTime * _speed);
                _playerState.StartMoving();
            }
            else // continue moving
            {
                _transform.position = Vector3.MoveTowards(_transform.position, _playerState.GetTarget(), Time.deltaTime * _speed);
            }
        }

        private void RotateAndSetTarget()
        {
            int currentRotation = RotateHelper.GetCurrentRotation(_transform.eulerAngles);
            if (_inputs.Left())
            {
                _playerState.SetTarget(_transform.position + Vector3.left * GameConstants.BlockScale);
                _transform.Rotate(new Vector3(0,  RotateHelper.RotateToLeft(currentRotation),0));
            }
            else if (_inputs.Right())
            {
                _playerState.SetTarget(_transform.position + Vector3.right * GameConstants.BlockScale);
                _transform.Rotate(new Vector3(0, RotateHelper.RotateToRight(currentRotation),0));
            }
            else if (_inputs.Backward())
            {
                _playerState.SetTarget(_transform.position + Vector3.back * GameConstants.BlockScale);
                _transform.Rotate(new Vector3(0, RotateHelper.RotateToBack(currentRotation),0));
            }
            else if (_inputs.Forward())
            {
                _playerState.SetTarget(_transform.position + Vector3.forward * GameConstants.BlockScale);
                _transform.Rotate(new Vector3(0, RotateHelper.RotateToFront(currentRotation),0));
            }
        
            _playerState.UpdateDirection(currentRotation);
        }
    }
}
