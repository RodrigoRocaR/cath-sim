using UnityEngine;

public class TiledMovementController
{
    private readonly Inputs _inputs;
    private readonly Transform _transform;
    private readonly float _speed;
    
    private Vector3 _target; // target position to move to
    private bool _moving; // is the player moving to another tile?
    private bool _isBlockInFront, _isWallInFront;

    public TiledMovementController(Transform transform, Inputs inputs, float speed)
    {
        _transform = transform;
        _speed = speed;
        _inputs = inputs;
    }
    
    public void Move()
    {
        if (_transform.position == _target) _moving = false; // we reached the target => reset to false
        if (!_moving && (_inputs.MultipleInputs() || !_inputs.AnyInputs())) return;
        
        // Movement
        if (!_moving)
        {
            if (!_inputs.AnyInputs()) return;
            RotateAndSetTarget();
            CheckForBlocksInFront();
            if (_isWallInFront) return;
            if (_isBlockInFront)
            {
                return;
            }
            
            _transform.position = Vector3.MoveTowards(_transform.position, _target, Time.deltaTime * _speed);
            _moving = true;
        }
        else // continue moving
        {
            _transform.position = Vector3.MoveTowards(_transform.position, _target, Time.deltaTime * _speed);
        }
    }

    public bool IsMoving()
    {
        return _moving;
    }

    private void CheckForBlocksInFront()
    {
        Vector3 pos = _transform.position;
        Vector3 verticalOffset = new Vector3(0, _transform.localScale.y*0.90f, 0);
        Vector3 fwd = _transform.TransformDirection(Vector3.forward);
        _isBlockInFront = Physics.Raycast(pos+verticalOffset, fwd, Level.BlockScale/2f);
        verticalOffset.y += Level.BlockScale;
        _isWallInFront = Physics.Raycast(pos+verticalOffset, fwd, Level.BlockScale/2f);
    }
    
    private void RotateAndSetTarget()
    {
        int currentRotation = GetRotation();
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
