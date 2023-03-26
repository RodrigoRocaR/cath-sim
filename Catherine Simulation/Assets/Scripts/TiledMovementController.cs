using UnityEngine;

public class TiledMovementController
{
    private readonly Inputs _inputs;
    private readonly Transform _transform;
    private readonly float _speed;

    private int _posx, _posy, _posz; // tiled position
    private Vector3 _target; // target position to move to
    private bool _isMoving; // is the player moving to another tile?
    private bool _isFalling;
    private bool _hasFoundation, _isBlockInFront, _isWallInFront;

    public TiledMovementController(Transform transform, Inputs inputs, float speed)
    {
        _transform = transform;
        _speed = speed;
        _inputs = inputs;
        _posx = (int) _transform.position.x;
        _posy = (int) _transform.position.y;
        _posz = (int) _transform.position.z;
    }
    
    public void Move()
    {
        if (_transform.position == _target){ 
            _isMoving = false; // we reached the target => reset to false
        }
        
        if (!_isMoving)
        {
            // Update tiled position
            _posx = (int)_target.x;
            _posy = (int)_target.y;
            _posz = (int)_target.z;
        }
        
        
        if (_isFalling || (!_isMoving && (_inputs.MultipleInputs() || !_inputs.AnyInputs()))) return;

        // Movement
        if (!_isMoving)
        {
            if (!_inputs.AnyInputs()) return;
            int currentRotation = GetRotation();
            RotateAndSetTarget(currentRotation);
            CheckForBlocksInFront();
            if (_isWallInFront) return;
            if (_isBlockInFront) return;
            if (!_hasFoundation) return;

                _transform.position = Vector3.MoveTowards(_transform.position, _target, Time.deltaTime * _speed);
            _isMoving = true;
        }
        else // continue moving
        {
            _transform.position = Vector3.MoveTowards(_transform.position, _target, Time.deltaTime * _speed);
        }
    }

    public bool IsMoving()
    {
        return _isMoving;
    }

    public bool UpdateIsFalling(Rigidbody rb) 
    {
        _isFalling = !rb.IsSleeping() && rb.velocity.y < -0.1;;
        return _isFalling;
    }

    private void CheckForBlocksInFront()
    {
        Vector3 checkPos = _target; // on top of the block we are going (even height)
        
        checkPos.y -= 1;
        _hasFoundation = Level.GetBlock(checkPos) != -1;
        checkPos.y += Level.BlockScale;
        _isBlockInFront = Level.GetBlock(checkPos) != -1;
        checkPos.y += Level.BlockScale;
        _isWallInFront = Level.GetBlock(checkPos) != -1;
        
    }

    private void RotateAndSetTarget(int currentRotation)
    {
        if (_inputs.Left())
        {
            _target = _transform.position + Vector3.left * Level.BlockScale;
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
            _target = _transform.position + Vector3.right * Level.BlockScale;
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
            _target = _transform.position + Vector3.back * Level.BlockScale;
            if (currentRotation == 180) return; 
            int rotation = (currentRotation == 0) ? 180 : currentRotation;
            _transform.Rotate(new Vector3(0, rotation,0));
        }
        else if (_inputs.Forward())
        {
            _target = _transform.position + Vector3.forward * Level.BlockScale;
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
