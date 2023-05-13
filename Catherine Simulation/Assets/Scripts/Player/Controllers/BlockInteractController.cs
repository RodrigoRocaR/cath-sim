using Blocks;
using Blocks.BlockTypes;
using LevelDS;
using UnityEngine;

namespace Player.Controllers
{
    public class BlockInteractController
    {
        private readonly Transform _transform;
        private readonly PlayerState _playerState;
        private readonly Inputs _input;

        private Vector3 _pressedBlockPlayerPos;

        public BlockInteractController(Transform playerTransform, PlayerState playerState, Inputs input)
        {
            _transform = playerTransform;
            _playerState = playerState;
            _input = input;
        }

        // Entry point
        public void InteractWithBlocks()
        {
            PressBlockBelow();
            MoveBlocks();
        }

        /*
         * Pulls / pushes the block in front of the player
         */
        private void MoveBlocks()
        {
            if (_input.Pull() && _playerState.CanMoveBlocks() && !IsBlockBehindPlayer())
            {
                IBlock block = Level.GetBlock(_transform.position + Vector3.up +
                                                   _playerState.GetDirection() * GameConstants.BlockScale);
                if (block == null) return;

                block.TriggerPull(_transform, _playerState);
            }
            else if (_input.Push() && _playerState.CanMoveBlocks() && !IsBlockBehindBlockInFront())
            {
                IBlock block = Level.GetBlock(_transform.position + Vector3.up +
                                                   _playerState.GetDirection() * GameConstants.BlockScale);
                if (block == null) return;

                block.TriggerPush(_transform, _playerState);
            }
        }

        private void PressBlockBelow()
        {
            if (Level.IsInBlockSpaceCoords(_transform.position) && _transform.position != _pressedBlockPlayerPos)
            {
                IBlock block = GetBlockBelowPlayer();
                if (block == null) return;
                _pressedBlockPlayerPos = _transform.position;
                block.OnPlayerStepOn();
            }
        }

        private bool IsBlockBehindPlayer()
        {
            return Level.GetBlockInt(_transform.position - _playerState.GetDirection() * GameConstants.BlockScale +
                                     Vector3.up) !=
                   GameConstants.EmptyBlock;
        }

        private bool IsBlockBehindBlockInFront()
        {
            return Level.GetBlockInt(_transform.position +
                                     _playerState.GetDirection() * (2 * GameConstants.BlockScale) +
                                     Vector3.up) != GameConstants.EmptyBlock;
        }

        private IBlock GetBlockBelowPlayer()
        {
            return Level.GetBlock(_transform.position + Vector3.down * GameConstants.BlockScale);
        }
    }
}