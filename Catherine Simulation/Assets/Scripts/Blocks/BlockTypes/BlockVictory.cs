using Player;
using UnityEngine;

namespace Blocks.BlockTypes
{
    public class BlockVictory : IBlock
    {
        
        
        
        // Can not be moved
        public void TriggerPull(Transform playerTransform, PlayerState playerState)
        {
        }

        public void TriggerPush(Transform playerTransform, PlayerState playerState)
        {
        }
    }
}