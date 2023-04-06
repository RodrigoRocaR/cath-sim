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
        private bool _isBeingPulled;
        
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
            Pull();
        }

        /**
     * This method should be called when the player pulls this block
     *  This triggers the pull. Moving the object and player
     */
        public void TriggerPull(Transform playerTransform, PlayerState playerState)
        {
            if (_isBeingPulled && !playerState.CanPull()) return;

            _playerTransform = playerTransform;
            _playerState = playerState;
            
            Vector3 playerPos = playerTransform.position;
            
            // Set up block
            _isBeingPulled = true;
            _blockProgress.TargetPos = playerPos + Vector3.up;
            _blockProgress.StartPos = transform.position;
        
            // Set up player
            _playerProgress.TargetPos = playerPos - _playerState.GetDirection() * Level.BlockScale;
            _playerProgress.StartPos = playerPos;
            
            _playerState.StartMovingBlock();
        }

        private void Pull()
        {
            if (_blockProgress.IsCompleted() && _playerProgress.IsCompleted()) // player and block should complete at the same time
            {
                Debug.Log(_blockProgress.Progress);
                ResetBlockState();
                _playerState.StopMovingBlock();
            }
        
            if (_isBeingPulled)
            {
                transform.position = _blockProgress.Lerp();
                _playerTransform.position = _playerProgress.Lerp();

                if (Vector3.Distance(_blockProgress.TargetPos, transform.position) <= 0.01f)
                {
                    var a = 0;
                }
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
