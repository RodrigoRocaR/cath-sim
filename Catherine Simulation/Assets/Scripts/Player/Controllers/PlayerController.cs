using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public float speed = 5.5f;
        public GameObject cameraGameObject;

        private Animator _animator;
        private Rigidbody _rb;
    
        private Inputs _inputs;
        private CameraTiled _cameraTiled;
        private TiledMovementController _tiledMovementController;
        private JumpController _jumpController;
        private AnimationsController _animationsController;
        private BlockInteractController _blockInteractController;
        private HangController _hangController;
        private PlayerState _playerState;
    
    
        void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _rb = GetComponent<Rigidbody>();
        
            _inputs = new Inputs();
            _playerState = new PlayerState();
            _cameraTiled = new CameraTiled(cameraGameObject, gameObject);
            
            // Child controllers
            _tiledMovementController = new TiledMovementController(transform, _inputs, _playerState, speed);
            _jumpController = new JumpController(transform, _inputs, _playerState, 0.85f);
            _animationsController = new AnimationsController(_animator, _playerState);
            _blockInteractController = new BlockInteractController(transform, _playerState, _inputs);
            _hangController = new HangController(transform, _playerState, _inputs, _cameraTiled);
        
            _playerState.UpdateDirection(transform.eulerAngles);
        }

        // Update is called once per frame
        void Update()
        {
            _inputs.UpdateInputs();
            _playerState.UpdateDirection(transform.eulerAngles);
            _tiledMovementController.Move();
            _jumpController.Jump();
            _blockInteractController.MoveBlocks();
            _hangController.Hang();
        }

        private void FixedUpdate()
        {
            _animationsController.UpdateAnimations();
            _playerState.UpdateIsFalling(_rb);
        }

        private void LateUpdate()
        {
            _cameraTiled.LateUpdate();
            _rb.useGravity = _playerState.GetGravityDesiredValue();
        }
    }
}
