using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.5f;

    private Animator _animator;
    private Rigidbody _rb;
    private TiledMovementController _tiledMovementController;
    private Inputs _inputs;
    private AnimationsController _animationsController;
    
    
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody>();
        
        _inputs = new Inputs();
        _tiledMovementController = new TiledMovementController(transform, _inputs, speed);
        _animationsController = new AnimationsController(_animator, _rb, _inputs);
    }

    // Update is called once per frame
    void Update()
    {
        _inputs.UpdateInputs();
        _tiledMovementController.Move();
    }

    private void FixedUpdate()
    {
        _animationsController.UpdateAnimations(_tiledMovementController.IsMoving());
        _tiledMovementController.UpdateIsFalling(_rb);
    }
}
