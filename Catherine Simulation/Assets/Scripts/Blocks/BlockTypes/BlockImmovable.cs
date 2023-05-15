using Player;
using UnityEngine;

namespace Blocks.BlockTypes
{
    public class BlockImmovable : IBlock
    {
        // Can not be moved
        public void TriggerPull(Transform playerTransform, PlayerState playerState)
        {
        }

        public void TriggerPush(Transform playerTransform, PlayerState playerState)
        {
        }
        
        // Does nothing
        public void OnPlayerStepOn()
        {
        }
    }
}