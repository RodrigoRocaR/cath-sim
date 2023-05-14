using Player;
using UnityEngine;

namespace Blocks.BlockTypes
{
    public class BlockVictory : IBlock
    {
        private GameObject _victoryCanvas;

        public BlockVictory(GameObject victoryCanvas)
        {
            _victoryCanvas = victoryCanvas;
        }
        
        public void OnPlayerStepOn()
        {
            // Game victory
            _victoryCanvas.SetActive(true);
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