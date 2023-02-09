using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.5f;
    public float blockScale;

    private bool _forward, _backward, _right, _left;
    private bool _multipleInputs, _anyInputs;
    private string _currentState; // for animations states
    private bool _isFalling;
    private bool _moving;
    private Vector3 _target;

    private Animator _animator;
    private Collider _collider;
    private Rigidbody _rb;
    private readonly AnimationsRegistry _animReg = new AnimationsRegistry();
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        _multipleInputs = (_forward && _backward) || (_forward && _right) || (_forward && _left) || 
                          (_backward && _right) || (_backward && _left) || (_right && _left);
        _anyInputs = _forward || _backward || _right || _left;
        _isFalling = !_rb.IsSleeping() && _rb.velocity.y < -0.1;
        Move();
    }

    private void FixedUpdate()
    {
        if ((!_anyInputs || _multipleInputs) && !_moving)
        {
            ChangeAnimationState(_animReg.PlayerIdle);
        }
        else
        {
            ChangeAnimationState(_animReg.PlayerRun);
        }
    }

    private void Move()
    {
        if (_multipleInputs)
        {
            return;
        }
        if (!_anyInputs && !_moving) return;
        
        // Movement
        if (transform.position == _target)
        {
            _moving = false;
        }
        
        if (!_moving)
        {
            if (!_anyInputs) return;
            RotateSelfToMove();
            transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * speed);
            _moving = true;
        }
        else // continue moving
        {
            transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * speed);
        }
    }

    private void RotateSelfToMove()
    {
        int currentRotation = GetRotation();
        if (_left)
        {
            _target = transform.position + Vector3.left * blockScale;
            if (currentRotation == 270) return; 
            int rotation = currentRotation switch
            {
                0 => -90,
                180 => 90,
                _ => 180
            };
            transform.Rotate(new Vector3(0, rotation,0));
        }
        else if (_right)
        {
            _target = transform.position + Vector3.right * blockScale;
            if (currentRotation == 90) return; 
            int rotation = currentRotation switch
            {
                0 => 90,
                180 => -90,
                _ => -180
            };
            transform.Rotate(new Vector3(0, rotation,0));
        }
        else if (_backward)
        {
            _target = transform.position + Vector3.back * blockScale;
            if (currentRotation == 180) return; 
            int rotation = (currentRotation == 0) ? 180 : currentRotation;
            transform.Rotate(new Vector3(0, rotation,0));
        }
        else if (_forward)
        {
            _target = transform.position + Vector3.forward * blockScale;
            if (currentRotation is > 1 or < -1) // face forward (0)
            {
                transform.Rotate(new Vector3(0, -currentRotation,0));
            }
        }
    }

    private int GetRotation()
    {
        float rotation = transform.eulerAngles.y;
        return rotation switch
        {
            < 272 and > 268 => 270,
            < 182 and > 178 => 180,
            < 92 and > 88 => 90,
            _ => 0
        };
    }
    
    private void GetInputs()
    {
        _forward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        _backward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        _right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        _left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
    }
    
    private void ChangeAnimationState(string newState)
    {
        if (_currentState == newState) return;
        _animator.Play(newState); // play animation
        _currentState = newState; 
    }
}
