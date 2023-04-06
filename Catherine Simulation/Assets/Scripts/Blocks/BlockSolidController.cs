using LevelDS;
using Player;
using Tools;
using UnityEngine;

namespace Blocks
{
    public class BlockSolidController : MonoBehaviour
    {
        private const float MoveDuration = 1f;

        private MoveLerp _blockProgress;
        private MoveLerp _playerProgress;
        private bool _isBeingMoved;
        
        private Transform _playerTransform;
        private PlayerState _playerState;
    
        // Start is called before the first frame update
        void Start()
        {
            _blockProgress = new MoveLerp(MoveDuration);
            _playerProgress = new MoveLerp(MoveDuration);
            ResetBlockState();
        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        /**
     * This method should be called when the player pulls this block
     *  This triggers the pull. Moving the object and player
     */
        public void TriggerPull(Transform playerTransform, PlayerState playerState)
        {
            if (_isBeingMoved || !playerState.CanMoveBlocks()) return; // to double check

            _playerTransform = playerTransform;
            _playerState = playerState;
            
            Vector3 playerPos = playerTransform.position;
            
            // Set up block
            _isBeingMoved = true;
            _blockProgress.TargetPos = playerPos + Vector3.up;
            _blockProgress.StartPos = transform.position;
        
            // Set up player
            _playerProgress.TargetPos = playerPos - _playerState.GetDirection() * Level.BlockScale;
            _playerProgress.StartPos = playerPos;
            
            _playerState.StartMovingBlock();
        }

        public void TriggerPush(Transform playerTransform, PlayerState playerState)
        {
            if (_isBeingMoved || !playerState.CanMoveBlocks()) return; // to double check

            _playerTransform = playerTransform;
            _playerState = playerState;
            
            Vector3 playerPos = playerTransform.position;
            
            // Set up block
            _isBeingMoved = true;
            _blockProgress.TargetPos = playerPos + _playerState.GetDirection() * (2 * Level.BlockScale) + Vector3.up;
            _blockProgress.StartPos = transform.position;
        
            // Set up player
            _playerProgress.TargetPos = playerPos;
            _playerProgress.StartPos = playerPos;
            
            _playerState.StartMovingBlock();
        }

        private void Move()
        {
            if (!_isBeingMoved) return;
            
            if (_blockProgress.IsCompleted() && _playerProgress.IsCompleted()) // player and block should complete at the same time
            {
                // Update block position
                Level.UpdateMovedBlock(_blockProgress.StartPos, _blockProgress.TargetPos);
                
                ResetBlockState();
                _playerState.StopMovingBlock();
            }
            else
            {
                transform.position = _blockProgress.Lerp();
                _playerTransform.position = _playerProgress.Lerp();
            }
        }

        private void ResetBlockState()
        {
            _isBeingMoved = false;
            _blockProgress.Reset();
            _playerProgress.Reset();
        }
    }
}
