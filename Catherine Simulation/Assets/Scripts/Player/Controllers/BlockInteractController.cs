using Blocks;
using LevelDS;
using UnityEngine;

namespace Player.Controllers
{
    public class BlockInteractController
    {
        private readonly Transform _transform;
        private readonly PlayerState _playerState;
        private readonly Inputs _input;


        public BlockInteractController(Transform playerTransform, PlayerState playerState, Inputs input)
        {
            _transform = playerTransform;
            _playerState = playerState;
            _input = input;
        }

        /*
         * Pulls the block in front of the player, if any
         */
        public void MoveBlocks()
        {
            if (_input.Pull() && _playerState.CanMoveBlocks() && !IsBlockBehindPlayer())
            {
                BlockSolidController blockSolidController = GetBlockController();
                if (blockSolidController == null) return;

                blockSolidController.TriggerPull(_transform, _playerState);
            }
            else if (_input.Push() && _playerState.CanMoveBlocks() && !IsBlockBehindBlockInFront())
            {
                BlockSolidController blockSolidController = GetBlockController();
                if (blockSolidController == null) return;

                blockSolidController.TriggerPush(_transform, _playerState);
            }
        }

        private BlockSolidController GetBlockController()
        {
            GameObject block =
                Level.GetBlock(_transform.position + Vector3.up + _playerState.GetDirection() * GameConstants.BlockScale);
            return block == null ? null : block.GetComponent<BlockSolidController>();
        }

        private bool IsBlockBehindPlayer()
        {
            return Level.GetBlockInt(_transform.position - _playerState.GetDirection() * GameConstants.BlockScale +
                                     Vector3.up) !=
                   GameConstants.EmptyBlock;
        }

        private bool IsBlockBehindBlockInFront()
        {
            return Level.GetBlockInt(_transform.position + _playerState.GetDirection() * (2 * GameConstants.BlockScale) +
                                     Vector3.up) != GameConstants.EmptyBlock;
        }
    }
}