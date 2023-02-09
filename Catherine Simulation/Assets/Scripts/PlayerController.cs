using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.5f;

    private bool _forward, _backward, _right, _left;
    private bool _multipleInputs, _anyInputs;
    private string _currentState; // for animations states
    private bool _isFalling;

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

    private void FixedUpdate() // for physics calculations
    {
        if (!_anyInputs || _multipleInputs)
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


        float currentRotation = transform.eulerAngles.y;
        if (_left)
        {
            if (currentRotation is > 271 or < 269) // face left (270)
            {
                transform.Rotate(new Vector3(0, -90,0));
            }
            transform.Translate(Time.deltaTime * speed * Vector3.forward); 
        }
        else if (_right)
        {
            if (currentRotation is > 91 or < 89) // face right (90)
            {
                transform.Rotate(new Vector3(0, 90,0));
            }
            transform.Translate(Time.deltaTime * speed * Vector3.forward); 
        }
        else if (_backward)
        {
            if (currentRotation is > 181 or < 179) // face backwards (180)
            {
                float rotation = (currentRotation == 0) ? 180 : currentRotation;
                transform.Rotate(new Vector3(0, rotation,0));
            }
            transform.Translate(Time.deltaTime * speed * Vector3.forward); 
        }
        else if (_forward)
        {
            if (currentRotation is > 1 or < -1) // face forward (0)
            {
                transform.Rotate(new Vector3(0, -currentRotation,0));
            }
            transform.Translate(Time.deltaTime * speed * Vector3.forward); 
        }
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
