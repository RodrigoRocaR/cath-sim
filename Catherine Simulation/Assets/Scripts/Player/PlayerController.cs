using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float speed = 5.5f;

        private Animator _animator;
        private Rigidbody _rb;
    
        private Inputs _inputs;
        private TiledMovementController _tiledMovementController;
        private JumpController _jumpController;
        private AnimationsController _animationsController;
        private BlockInteractController _blockInteractController;
        private PlayerState _playerState;
    
    
        void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _rb = GetComponent<Rigidbody>();
        
            _inputs = new Inputs();
            _playerState = new PlayerState();
            _tiledMovementController = new TiledMovementController(transform, _inputs, _playerState, speed);
            _jumpController = new JumpController(transform, _inputs, _playerState, 0.85f);
            _animationsController = new AnimationsController(_animator, _playerState);
            _blockInteractController = new BlockInteractController(transform, _playerState, _inputs);
        
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
        }

        private void FixedUpdate()
        {
            _animationsController.UpdateAnimations();
            _playerState.UpdateIsFalling(_rb);
        }
    }
}
