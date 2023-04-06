using Blocks;
using LevelDS;
using UnityEngine;

namespace Player
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
        public void Pull()
        {
            if (_input.Pull() && _playerState.CanPull())
            {
                GameObject block = Level.GetBlock(_transform.position + Vector3.up + _playerState.GetDirection() * Level.BlockScale);
                if (block == null) return;
                
                BlockSolidController blockSolidController = block.GetComponent<BlockSolidController>();
                blockSolidController.TriggerPull(_transform, _playerState);
            }
            
        }
        
        
    }
}
