using UnityEngine;

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
        if (_transform.position == _playerState.Target){ 
            _playerState.IsMoving = false; // we reached the target => reset to false
        }
        
        if (_playerState.IsFalling || (!_playerState.IsMoving && (_inputs.MultipleInputs() || !_inputs.AnyInputs()))) return;

        // Movement
        if (!_playerState.IsMoving)
        {
            if (!_inputs.AnyInputs()) return;
            
            int currentRotation = GetRotation();
            RotateAndSetTarget(currentRotation);
            _playerState.CheckForBlocksInFront();
            
            if (_playerState.IsWallInFront) return;
            if (_playerState.IsBlockInFront) return;
            if (!_playerState.HasFoundation) return;

            _transform.position = Vector3.MoveTowards(_transform.position, _playerState.Target, Time.deltaTime * _speed);
            _playerState.IsMoving = true;
        }
        else // continue moving
        {
            _transform.position = Vector3.MoveTowards(_transform.position, _playerState.Target, Time.deltaTime * _speed);
        }
    }

    private void RotateAndSetTarget(int currentRotation)
    {
        if (_inputs.Left())
        {
            _playerState.Target = _transform.position + Vector3.left * Level.BlockScale;
            if (currentRotation == 270) return; 
            int rotation = currentRotation switch
            {
                0 => -90,
                180 => 90,
                _ => 180
            };
            _transform.Rotate(new Vector3(0, rotation,0));
        }
        else if (_inputs.Right())
        {
            _playerState.Target = _transform.position + Vector3.right * Level.BlockScale;
            if (currentRotation == 90) return; 
            int rotation = currentRotation switch
            {
                0 => 90,
                180 => -90,
                _ => -180
            };
            _transform.Rotate(new Vector3(0, rotation,0));
        }
        else if (_inputs.Backward())
        {
            _playerState.Target = _transform.position + Vector3.back * Level.BlockScale;
            if (currentRotation == 180) return; 
            int rotation = (currentRotation == 0) ? 180 : currentRotation;
            _transform.Rotate(new Vector3(0, rotation,0));
        }
        else if (_inputs.Forward())
        {
            _playerState.Target = _transform.position + Vector3.forward * Level.BlockScale;
            if (currentRotation is > 1 or < -1) // face forward (0)
            {
                _transform.Rotate(new Vector3(0, -currentRotation,0));
            }
        }
    }

    private int GetRotation()
    {
        float rotation = _transform.eulerAngles.y;
        return rotation switch
        {
            < 272 and > 268 => 270,
            < 182 and > 178 => 180,
            < 92 and > 88 => 90,
            _ => 0
        };
    }
}
