using LevelDS;
using Player;
using Tools.Lerps;
using UnityEngine;

namespace Blocks.BlockTypes
{
    public abstract class MovableBlock : IBlock
    {
        private MoveLerp _blockProgress;
        private MoveLerp _playerProgress;
        private bool _isBeingMoved;

        private Transform _playerTransform;
        private Transform _blockTransform;
        private PlayerState _playerState;

        protected MovableBlock(Transform blockTransform, float moveDuration)
        {
            _blockTransform = blockTransform;
            _blockProgress = new MoveLerp(moveDuration);
            _playerProgress = new MoveLerp(moveDuration);
        }

        public void TriggerPull(Transform playerTransform, PlayerState playerState, bool goingToHang = false)
        {
            if (_isBeingMoved || !playerState.CanMoveBlocks()) return; // to double check

            _playerTransform = playerTransform;
            _playerState = playerState;

            Vector3 playerPos = playerTransform.position;

            // Set up block
            _isBeingMoved = true;
            _blockProgress.Setup(_blockTransform.position, playerPos + Vector3.up);

            // Set up player
            if (!goingToHang)
                _playerProgress.Setup(playerPos, playerPos - _playerState.GetDirection() * GameConstants.BlockScale);
            else playerState.StartMovingAndHang();

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
            _blockProgress.Setup(_blockTransform.position,
                playerPos + _playerState.GetDirection() * (2 * GameConstants.BlockScale) + Vector3.up);

            // Set up player
            _playerProgress.Setup(playerPos, playerPos);

            _playerState.StartMovingBlock();
        }

        public void UpdatePostionIfMoved()
        {
            if (!_isBeingMoved) return;

            if (_blockProgress.IsCompleted() &&
                _playerProgress.IsCompleted()) // player and block should complete at the same time
            {
                // Update block position
                Level.UpdateMovedBlock(_blockProgress.GetStart(), _blockProgress.GetEnd());

                ResetBlockState();
                _playerState.StopMovingBlock();
            }
            else
            {
                _blockTransform.position = _blockProgress.Lerp();
                _playerTransform.position = _playerProgress.Lerp();
            }
        }

        public virtual void OnPlayerStepOn()
        {
        }

        private void ResetBlockState()
        {
            _isBeingMoved = false;
            _blockProgress.Reset();
            _playerProgress.Reset();
        }
    }
}