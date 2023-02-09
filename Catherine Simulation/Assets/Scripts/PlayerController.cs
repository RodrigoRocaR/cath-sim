using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.5f;

    private bool _forward, _backward, _right, _left;
    private bool _multipleInputs, _anyInputs;
    private string _currentState; // for animations states
    private float _distToGround;

    private Animator _animator;
    private Collider _collider;
    private Rigidbody _rb;
    private readonly AnimationsRegistry _animReg = new AnimationsRegistry();
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
        _distToGround = _collider.bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        _forward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        _backward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        _right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        _left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        _multipleInputs = !(_forward && _backward) && !(_forward && _right) && !(_forward && _left) && 
                          !(_backward && _right) && !(_backward && _left) && !(_right && _left);
        _anyInputs = _forward || _backward || _right || _left;
        Move();
    }

    private void FixedUpdate() // for physics calculations
    {
        if (!_anyInputs)
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
        int currentRotation = (int) transform.eulerAngles.y;
        if (_left)
        {
            if (currentRotation != 270) // face left
            {
                transform.Rotate(new Vector3(0, -90,0));
            }
            transform.Translate(Time.deltaTime * speed * Vector3.forward); 
        }
        else if (_right)
        {
            if (currentRotation != 90) // face right
            {
                transform.Rotate(new Vector3(0, 90,0));
            }
            transform.Translate(Time.deltaTime * speed * Vector3.forward); 
        }
        else if (_backward)
        {
            if (currentRotation != 180) // face backwards
            {
                int rotation = (currentRotation == 0) ? 180 : currentRotation;
                transform.Rotate(new Vector3(0, rotation,0));
            }
            transform.Translate(Time.deltaTime * speed * Vector3.forward); 
        }
        else if (_forward)
        {
            if (currentRotation != 0) // face forward
            {
                transform.Rotate(new Vector3(0, -currentRotation,0));
            }
            transform.Translate(Time.deltaTime * speed * Vector3.forward); 
        }
    }
    
    private bool IsGrounded() {
        return Physics.Raycast(transform.position, Vector3.down, _distToGround + 0.1f);
    }

    void ChangeAnimationState(string newState)
    {
        if (_currentState == newState) return;
        _animator.Play(newState); // play animation
        _currentState = newState; 
    }
}
