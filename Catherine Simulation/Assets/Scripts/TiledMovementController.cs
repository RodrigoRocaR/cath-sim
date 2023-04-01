using Unity.VisualScripting;
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
        if (_transform.position == _playerState.GetTarget()){ 
            _playerState.StopMoving(); // we reached the target => reset to false
        }
        
        if (_playerState.IsJumping() || _playerState.IsFalling() ||
            (!_playerState.IsMoving() && (_inputs.Jump() || _inputs.MultipleInputs() || !_inputs.AnyInputs()))) return;

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
        int currentRotation = _playerState.GetRotation(_transform.eulerAngles);
        if (_inputs.Left())
        {
            _playerState.SetTarget(_transform.position + Vector3.left * Level.BlockScale);
            _transform.Rotate(new Vector3(0, RotateToLeft(currentRotation),0));
        }
        else if (_inputs.Right())
        {
            _playerState.SetTarget(_transform.position + Vector3.right * Level.BlockScale);
            _transform.Rotate(new Vector3(0, RotateToRight(currentRotation),0));
        }
        else if (_inputs.Backward())
        {
            _playerState.SetTarget(_transform.position + Vector3.back * Level.BlockScale);
            _transform.Rotate(new Vector3(0, RotateToBack(currentRotation),0));
        }
        else if (_inputs.Forward())
        {
            _playerState.SetTarget(_transform.position + Vector3.forward * Level.BlockScale);
            _transform.Rotate(new Vector3(0, RotateToFront(currentRotation),0));
        }
        
        _playerState.UpdateDirection(currentRotation);
    }


    private int RotateToLeft(int currentRotation)
    {
        return currentRotation switch
        {
            0 => -90,
            180 => 90,
            270 => 0,
            _ => 180
        };
    }

    private int RotateToRight(int currentRotation)
    {
        return currentRotation switch
        {
            0 => 90,
            90 => 0,
            180 => -90,
            _ => -180
        };
    }

    private int RotateToBack(int currentRotation)
    {
        return currentRotation switch
        {
            0 => 180,
            180 => 0,
            _ => currentRotation
        };
    }

    private int RotateToFront(int currentRotation)
    {
        return currentRotation switch
        {
            >1 or <-1 => -currentRotation,
            _ => 0
        };
    }
}
