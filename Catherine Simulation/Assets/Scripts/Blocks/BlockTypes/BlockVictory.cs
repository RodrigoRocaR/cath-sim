using Player;
using UnityEngine;

namespace Blocks.BlockTypes
{
    public class BlockVictory : IBlock
    {
        
        public void OnPlayerStepOn()
        {
            // Game victory
        }

        // Can not be moved
        public void TriggerPull(Transform playerTransform, PlayerState playerState)
        {
        }

        public void TriggerPush(Transform playerTransform, PlayerState playerState)
        {
        }
    }
}