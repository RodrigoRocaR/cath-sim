using LevelDS;
using UnityEngine;

namespace Player
{
    public class PlayerState
    {

        private Vector3 _direction;
        private Vector3 _target;
    
        private bool _isMoving;
        private bool _isFalling;
    
        private bool _hasFoundation;
        private bool _isBlockInFront;
        private bool _isWallInFront;
        private bool _isBlockBelow;
    
        private bool _isJumping;

        private bool _isMovingBlock;

        private bool _isHangingOnBorder;
        private bool _isDroppingOnBorder;
        private bool _isGettingUpFromBorder;

        // Direction ------------------
        public Vector3 GetDirection()
        {
            return _direction;
        }

        public void SetDirection(Vector3 direction)
        {
            _direction = direction;
        }
    
        public void UpdateDirection(int rotation) // rotation obtained from GetRotation method
        {
            _direction = rotation switch
            {
                0 => Vector3.forward,
                90 => Vector3.right,
                180 => Vector3.back,
                _ => Vector3.left
            };
        }

        public void UpdateDirection(Vector3 eulerAngles)
        {
            UpdateDirection(GetRotation(eulerAngles));
        }
    
        public int GetRotation(Vector3 eulerAngles)
        {
            float rotation = eulerAngles.y;
            return rotation switch
            {
                < 272 and > 268 => 270,
                < 182 and > 178 => 180,
                < 92 and > 88 => 90,
                _ => 0
            };
        }
    
        // Target ------------------
        public void SetTarget(Vector3 pos)
        {
            _target = pos;
        }

        public Vector3 GetTarget()
        {
            return _target;
        }

        // Movement ------------------
        public void StartMoving()
        {
            _isMoving = true;
        }

        public void StopMoving()
        {
            _isMoving = false;
        }

        public bool IsMoving()
        {
            return _isMoving;
        }
    
        // Falling ------------------
        public bool IsFalling()
        {
            return _isFalling;
        }
    
        public void UpdateIsFalling(Rigidbody rb) 
        {
            _isFalling = !rb.IsSleeping() && rb.velocity.y < -0.1;;
        }
    
        // Block detection ------------------
        public bool CanMove()
        {
            return _hasFoundation && !_isBlockInFront && !_isWallInFront && !_isMovingBlock;
        }
    
        public void CheckBlocksTarget()
        {
            CheckBlocks(_target); // on top of the block we are going (even height)
        }

        public void CheckBlocks(Vector3 pos)
        {
            pos.y -= 1 + Level.BlockScale;
            _isBlockBelow = Level.GetBlockInt(pos) != Level.EmptyBlock; // Block below ground
            pos.y += Level.BlockScale;
            _hasFoundation = Level.GetBlockInt(pos) != Level.EmptyBlock; // Ground Level
            pos.y += Level.BlockScale;
            _isBlockInFront = Level.GetBlockInt(pos) != Level.EmptyBlock; // Block in front
            pos.y += Level.BlockScale;
            _isWallInFront = Level.GetBlockInt(pos) != Level.EmptyBlock && _isBlockInFront; // Block on top in front
        }
    
    
        // Jump ------------------
        public Vector3 SetJumpTarget(Vector3 jumpStart)
        {
            // Set target in front and check blocks
            _target = jumpStart + _direction * Level.BlockScale;
            CheckBlocksTarget();

            if (_isWallInFront) // same pos as start, jump on same tile
            {
                _target = jumpStart;
            }
            else if (_isBlockInFront) // 1 block up
            {
                _target += Vector3.up * Level.BlockScale;
            }
            else if (_isBlockBelow && !_hasFoundation) // 1 block down
            {
                _target += Vector3.down * Level.BlockScale;
            }

            return _target;
        }

        public void StartJumping()
        {
            _isJumping = true;
        }

        public void StopJumping()
        {
            _isJumping = false;
        }

        public bool IsJumping()
        {
            return _isJumping;
        }

        public bool CanJump()
        {
            return !_isJumping && !_isMoving && !_isMovingBlock;
        }
        
        // Move blocks
        public void StartMovingBlock()
        {
            _isMovingBlock = true;
        }

        public void StopMovingBlock()
        {
            _isMovingBlock = false;
        }

        public bool IsMovingBlocks()
        {
            return _isMovingBlock;
        }

        public bool CanMoveBlocks()
        {
            return !_isJumping && !_isMoving && !_isMovingBlock;
        }
        
        // Hang on a border
        public void HangOnBorder()
        {
            _isHangingOnBorder = true;
        }
        
        public void StopHanging()
        {
            _isHangingOnBorder = false;
        }

        public bool IsHangingOnBorder()
        {
            return _isHangingOnBorder;
        }

        public void DropOnBorder()
        {
            _isDroppingOnBorder = true;
        }
        
        public void StopDroppingOnBorder()
        {
            _isDroppingOnBorder = false;
        }
        
        public bool IsDroppingOnBorder()
        {
            return _isDroppingOnBorder;
        }

        public void GetUpFromBorder()
        {
            _isGettingUpFromBorder = true;
        }

        public void StopGettingUpFromBorder()
        {
            _isGettingUpFromBorder = false;
        }

        public bool IsGettingUpFromBorder()
        {
            return _isGettingUpFromBorder;
        }

        public bool CanHangOnBorder()
        {
            return !_hasFoundation && !_isBlockInFront && !_isBlockBelow 
                   && !_isDroppingOnBorder && !_isGettingUpFromBorder && !_isHangingOnBorder;
        }

        public bool IsPerformingHangingAction()
        {
            return !_isDroppingOnBorder || !_isGettingUpFromBorder || !_isHangingOnBorder;
        }

        public void StopHangAction()
        {
            _isDroppingOnBorder = false;
            _isHangingOnBorder = false;
            _isGettingUpFromBorder = false;
        }
    }
}