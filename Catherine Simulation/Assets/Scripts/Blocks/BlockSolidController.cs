using LevelDS;
using Tools;
using UnityEngine;

namespace Blocks
{
    public class BlockSolidController : MonoBehaviour
    {
        private const float MoveDuration = 1f;
    
        private MoveLerp _blockProgress;
        private MoveLerp _playerProgress;
        private bool _isBeingPulled;
        private Transform _playerTransform;
    
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
            Pull();
        }

        /**
     * This method should be called when the player pulls this block
     *  This triggers the pull. Moving the object and player
     */
        public void TriggerPull(Transform playerTransform, Vector3 playerDirection)
        {
            if (_isBeingPulled) return;
            if (Vector3.Distance(playerTransform.position, transform.position) > Level.BlockScale)
            {
                Debug.LogError("Pulling block wrong. Player pos: " + playerTransform.position + 
                               "; Block pos: " + transform.position);
                return;
            }
        
            Vector3 playerPos = playerTransform.position;
        
            // Set up block
            _isBeingPulled = true;
            _blockProgress.TargetPos = playerPos;
            _blockProgress.StartPos = transform.position;
        
            // Set up player
            _playerProgress.TargetPos = playerPos - playerDirection;
            _playerProgress.StartPos = playerPos;
        }

        private void Pull()
        {
            if (_blockProgress.IsCompleted() && _playerProgress.IsCompleted()) // player and block should complete at the same time
            {
                ResetBlockState();
            }
        
            if (_isBeingPulled)
            {
                transform.position = _blockProgress.Lerp();
                _playerTransform.position = _playerProgress.Lerp();
            }
        }

        private void ResetBlockState()
        {
            _isBeingPulled = false;
            _blockProgress.Reset();
            _playerProgress.Reset();
        }
    }
}
