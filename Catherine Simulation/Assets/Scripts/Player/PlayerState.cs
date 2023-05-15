using LevelDS;
using Tools;
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
        private bool _isMovingBlockAndHang;

        private bool _isHangingOnBorder;
        private bool _isDroppingOnBorder;
        private bool _isGettingUpFromBorder;
        private bool _isDroppingFromHanging;
        
        private bool _gravityDesiredValue = true;

        public void Reset()
        {
            _direction = Vector3.forward;
            _target = Vector3.positiveInfinity;
            
            _isMoving = false;
            _isFalling = false;
            
            _hasFoundation = false;
            _isBlockBelow = false;
            _isWallInFront = false;
            _isBlockBelow = false;

            _isJumping = false;

            _isMovingBlock = false;
            _isMovingBlockAndHang = false;

            _isHangingOnBorder = false;
            _isDroppingOnBorder = false;
            _isGettingUpFromBorder = false;
            _isDroppingFromHanging = false;
            
            _gravityDesiredValue = true;
        }
        
        
        // Direction ------------------
        public Vector3 GetDirection()
        {
            return _direction;
        }

        public void SetDirection(Vector3 direction)
        {
            _direction = direction;
        }
    
        public void UpdateDirection(int rotation) // rotation obtained from GetCurrentRotation method
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
            UpdateDirection(RotateHelper.GetCurrentRotation(eulerAngles));
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
            pos.y -= 1 + GameConstants.BlockScale;
            _isBlockBelow = Level.GetBlockInt(pos) != GameConstants.EmptyBlock; // Block below ground
            pos.y += GameConstants.BlockScale;
            _hasFoundation = Level.GetBlockInt(pos) != GameConstants.EmptyBlock; // Ground Level
            pos.y += GameConstants.BlockScale;
            _isBlockInFront = Level.GetBlockInt(pos) != GameConstants.EmptyBlock; // Block in front
            pos.y += GameConstants.BlockScale;
            _isWallInFront = Level.GetBlockInt(pos) != GameConstants.EmptyBlock && _isBlockInFront; // Block on top in front
        }

        public bool IsBlockInFront()
        {
            return _isBlockInFront;
        }
    
    
        // Jump ------------------
        public Vector3 SetJumpTarget(Vector3 jumpStart)
        {
            // Set target in front and check blocks
            _target = jumpStart + _direction * GameConstants.BlockScale;
            CheckBlocksTarget();

            if (_isWallInFront) // same pos as start, jump on same tile
            {
                _target = jumpStart;
            }
            else if (_isBlockInFront) // 1 block up
            {
                _target += Vector3.up * GameConstants.BlockScale;
            }
            else if (_isBlockBelow && !_hasFoundation) // 1 block down
            {
                _target += Vector3.down * GameConstants.BlockScale;
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
            return !IsPerformingAction();
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

        public void StartMovingAndHang()
        {
            _isMovingBlockAndHang = true;
        }
        
        public void StopMovingAndHang()
        {
            _isMovingBlockAndHang = false;
        }
        
        public bool IsMovingBlockAndHang()
        {
            return _isMovingBlockAndHang;
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

        public bool CanDropOnBorder()
        {
            return !IsPerformingAction() &&
                   !_hasFoundation && !_isBlockInFront && !_isBlockBelow; // block layout correct
        }

        public bool IsPerformingHangingAction()
        {
            return _isDroppingOnBorder || _isGettingUpFromBorder || _isHangingOnBorder || _isDroppingFromHanging || _isMovingBlockAndHang;
        }

        public bool IsPerformingAction()
        {
            return _isMoving || _isJumping || _isFalling || _isMovingBlock || IsPerformingHangingAction();
        }

        public void DropFromHanging()
        {
            _isDroppingFromHanging = true;
        }

        public void StopDropFromHanging()
        {
            _isDroppingFromHanging = false;
        }

        public bool IsDroppingFromHanging()
        {
            return _isDroppingFromHanging;
        }

        public void GravityOn()
        {
            _gravityDesiredValue = true;
        }

        public void GravityOff()
        {
            _gravityDesiredValue = false;
        }

        public bool GetGravityDesiredValue()
        {
            return _gravityDesiredValue;
        }
    }
}