using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.5f;

    private Animator _animator;
    private Rigidbody _rb;
    
    private Inputs _inputs;
    private TiledMovementController _tiledMovementController;
    private JumpController _jumpController;
    private AnimationsController _animationsController;
    private PlayerState _playerState;
    
    
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody>();
        
        _inputs = new Inputs();
        _playerState = new PlayerState();
        _tiledMovementController = new TiledMovementController(transform, _inputs, _playerState, speed);
        _jumpController = new JumpController(transform, _inputs, _playerState, 1.15f);
        _animationsController = new AnimationsController(_animator, _rb, _inputs);
    }

    // Update is called once per frame
    void Update()
    {
        _inputs.UpdateInputs();
        _tiledMovementController.Move();
        _jumpController.Jump();
    }

    private void FixedUpdate()
    {
        _animationsController.UpdateAnimations(_playerState.IsMoving);
        _playerState.UpdateIsFalling(_rb);
    }
}
