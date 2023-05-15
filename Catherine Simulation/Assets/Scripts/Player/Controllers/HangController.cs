using UnityEngine;
using LevelDS;
using Tools.Lerps;

namespace Player.Controllers
{
    public class HangController
    {
        // Distance constants
        private const float DistancePercentageToBlock = 1.5f;

        private const float HorizontalOffset =
            GameConstants.BlockScale / 2f * DistancePercentageToBlock;

        private const float VerticalOffset = GameConstants.BlockScale / (0.75f * GameConstants.BlockScale);

        private const float CorneringForwardDistanceFromMidPosToTarget = GameConstants.BlockScale * 0.75f;
        private const float CorneringBackwardDistanceFromMidPosToTarget = GameConstants.BlockScale * 0.25f;

        // Times
        private const float FromBlockToEdgeTime = 0.75f;
        private const float FromBlockToEdgeTimeWhenGrabbing = 0.35f;
        private const float FromEdgeToHangTime = 0.35f;

        private const float HangSlideTime = 0.6f;

        private const float HangSlideToCornerEdgeTime = 0.3f;
        private const float HangSlideFromCornerEdgeToTargetTime = 0.3f;

        private const float SeparateFromBorderToFallTime = 0.35f;

        private readonly Inputs _inputs;
        private readonly Transform _transform;
        private readonly PlayerState _playerState;
        private readonly CameraTiled _cameraTiled;

        private MultiMoveLerp _multiMoveLerp;
        private bool _slidingLeft;

        public HangController(Transform transform, PlayerState playerState, Inputs inputs, CameraTiled cameraTiled)
        {
            _transform = transform;
            _inputs = inputs;
            _playerState = playerState;
            _cameraTiled = cameraTiled;
        }

        // Entry point
        public void Hang()
        {
            _playerState.CheckBlocksTarget();
            if (_multiMoveLerp != null && _playerState.IsPerformingHangingAction())
            {
                if (_multiMoveLerp.IsCompleted())
                {
                    StopHangAction();
                    _multiMoveLerp = null;
                }
                else
                {
                    _transform.position = _multiMoveLerp.Lerp();
                }
            }
            else if (_playerState.IsMovingBlockAndHang())
            {
                SetupDropOnBorderAfterHang();
                _playerState.GravityOff();
            }
            else if (_playerState.CanDropOnBorder() && _inputs.AnyInputs())
            {
                SetupDropOnBorder();
                _playerState.GravityOff();
                RotateToFaceBlockOnDrop();
            }
            else if (_playerState.IsHangingOnBorder() && _inputs.Horizontal()) // sliding left / right
            {
                SetupHangingSlide();
            }
            else if (_playerState.IsHangingOnBorder() && _inputs.Forward()) // get back up
            {
                if (!Level.IsBlock(PlayerPos2BlockInFrontPos(_transform.position) +
                                   Vector3.up * GameConstants.BlockScale))
                {
                    SetupGetBack();
                }
            }
            else if (_playerState.IsHangingOnBorder() && _inputs.Backward()) // drop and begin to fall
            {
                SetupDropFromHanging();
            }
        }

        private void SetupDropFromHanging()
        {
            Vector3 playerPos = _transform.position;
            Vector3 targetPos = playerPos - _playerState.GetDirection() * (0.25f * GameConstants.BlockScale);

            _multiMoveLerp = _multiMoveLerp = new MultiMoveLerp(
                new[] { SeparateFromBorderToFallTime },
                new[]
                {
                    playerPos,
                    targetPos
                });


            _playerState.StopHanging();
            _playerState.DropFromHanging();
        }

        private void SetupGetBack()
        {
            Vector3 playerPos = _transform.position;
            Vector3 blockEdge = playerPos + Vector3.up * VerticalOffset;
            Vector3 targetPos = blockEdge + _playerState.GetDirection() * HorizontalOffset;

            _playerState.GetUpFromBorder();
            _playerState.StopHanging();

            _multiMoveLerp = new MultiMoveLerp(
                new[] { FromEdgeToHangTime, FromBlockToEdgeTime },
                new[]
                {
                    playerPos,
                    blockEdge,
                    targetPos
                });
        }

        private void SetupHangingSlide()
        {
            Vector3 targetHangDirection = GetTargetHangDirection();
            Vector3 playerLookingDirection = _playerState.GetDirection();
            Vector3 playerPos = _transform.position;
            Vector3 targetPos = playerPos + targetHangDirection * GameConstants.BlockScale;
            Vector3 checkPos = PlayerPos2BlockInFrontPos(playerPos);


            checkPos += targetHangDirection * GameConstants.BlockScale;
            bool isBlockSameLevel = Level.IsBlock(checkPos);
            bool isBlockInTheWay = Level.IsBlock(checkPos - playerLookingDirection * GameConstants.BlockScale);

            if (isBlockSameLevel && !isBlockInTheWay) // Slide horizontally on the same wall
            {
                _multiMoveLerp = new MultiMoveLerp(
                    new[] { HangSlideTime },
                    new[] { playerPos, targetPos }
                );
                return;
            }

            // Corner case (literally)
            Vector3 midPos = playerPos + targetHangDirection * HorizontalOffset;

            if (isBlockInTheWay) // corner backward
            {
                // the block is in the way so the mid pos cant be in the edge
                midPos -= targetHangDirection * GameConstants.BlockScale / 2;
                targetPos = midPos - _playerState.GetDirection() * CorneringBackwardDistanceFromMidPosToTarget;
                _multiMoveLerp = new MultiMoveLerp(
                    new[] { HangSlideToCornerEdgeTime, HangSlideFromCornerEdgeToTargetTime },
                    new[]
                    {
                        playerPos,
                        midPos,
                        targetPos
                    }
                );
                RotateToFaceBlockCorner(false);
                _cameraTiled.RotateCameraSmooth(_playerState.GetDirection(), targetPos, _multiMoveLerp.GetTotalDuration());
                return;
            }

            // corner forward
            targetPos = midPos + _playerState.GetDirection() * CorneringForwardDistanceFromMidPosToTarget;
            _multiMoveLerp = new MultiMoveLerp(
                new[] { HangSlideToCornerEdgeTime, HangSlideFromCornerEdgeToTargetTime },
                new[]
                {
                    playerPos,
                    midPos,
                    targetPos
                }
            );
            RotateToFaceBlockCorner(true);
            _cameraTiled.RotateCameraSmooth(_playerState.GetDirection(), targetPos, _multiMoveLerp.GetTotalDuration());
        }


        private void SetupDropOnBorder()
        {
            FixDirection();

            Vector3 playerPos = _transform.position;

            Vector3 blockEdge = playerPos + _playerState.GetDirection() * HorizontalOffset;
            Vector3 hangingPos = blockEdge;
            hangingPos.y -= VerticalOffset;

            _playerState.DropOnBorder();
            _multiMoveLerp = new MultiMoveLerp(
                new[] { FromBlockToEdgeTime, FromEdgeToHangTime },
                new[]
                {
                    playerPos,
                    blockEdge,
                    hangingPos
                });
            _cameraTiled.RotateCameraSmooth(_playerState.GetDirection() * -1, hangingPos, _multiMoveLerp.GetTotalDuration());
        }
        
        private void SetupDropOnBorderAfterHang()
        {
            Vector3 playerPos = _transform.position;

            Vector3 blockEdge = playerPos - _playerState.GetDirection() * HorizontalOffset;
            Vector3 hangingPos = blockEdge;
            hangingPos.y -= VerticalOffset;

            _playerState.DropOnBorder();
            _multiMoveLerp = new MultiMoveLerp(
                new[] { FromBlockToEdgeTimeWhenGrabbing, FromEdgeToHangTime },
                new[]
                {
                    playerPos,
                    blockEdge,
                    hangingPos
                });
            _cameraTiled.RotateCameraSmooth(_playerState.GetDirection(), hangingPos, _multiMoveLerp.GetTotalDuration());
        }

        private void RotateToFaceBlockOnDrop()
        {
            _transform.Rotate(new Vector3(0, 180, 0));
            _playerState.UpdateDirection(_transform.eulerAngles);
        }

        private void RotateToFaceBlockCorner(bool forward)
        {
            if (_slidingLeft)
            {
                int rotation = forward ? 90 : -90;
                _transform.Rotate(new Vector3(0, rotation, 0));
            }
            else
            {
                int rotation = forward ? -90 : 90;
                _transform.Rotate(new Vector3(0, rotation, 0));
            }

            _playerState.UpdateDirection(_transform.eulerAngles);
        }

        private void StopHangAction()
        {
            if (_playerState.IsDroppingOnBorder()) // ended dropping
            {
                _playerState.StopMovingAndHang();
                _playerState.StopDroppingOnBorder();
                _playerState.HangOnBorder();
            }
            else if (_playerState.IsGettingUpFromBorder()) // ended getting up
            {
                _playerState.StopGettingUpFromBorder();
                _playerState.GravityOn();
                _cameraTiled.ResetCameraRotation();
            }
            else if (_playerState.IsDroppingFromHanging())
            {
                _playerState.GravityOn();
                _playerState.StopDropFromHanging();
                _cameraTiled.ResetCameraRotation();
            }
            // if its hanging we don't do anything
        }

        private void FixDirection()
        {
            Vector3 actualDirection = (_playerState.GetTarget() - _transform.position) / GameConstants.BlockScale;
            if (_playerState.GetDirection() != actualDirection)
            {
                _playerState.SetDirection(actualDirection);
            }
        }

        private Vector3 PlayerPos2BlockInFrontPos(Vector3 hangingPos)
        {
            Vector3 direction = _playerState.GetDirection();
            // Fix horizontal coords
            if (direction == Vector3.forward)
            {
                hangingPos.z += HorizontalOffset;
            }
            else if (direction == Vector3.back)
            {
                hangingPos.z -= HorizontalOffset;
            }
            else if (direction == Vector3.right)
            {
                hangingPos.x += HorizontalOffset;
            }
            else if (direction == Vector3.left)
            {
                hangingPos.x -= HorizontalOffset;
            }

            // Fix vertical coords
            hangingPos.y += GameConstants.BlockScale / 2f - VerticalOffset / 2f;

            return hangingPos;
        }

        private Vector3 GetTargetHangDirection()
        {
            Vector3 playerDir = _playerState.GetDirection();
            Vector3 hangDir;
            if (playerDir == Vector3.forward)
            {
                hangDir = _inputs.Right() ? Vector3.right : Vector3.left;
            }
            else if (playerDir == Vector3.right)
            {
                hangDir = _inputs.Right() ? Vector3.back : Vector3.forward;
            }
            else if (playerDir == Vector3.left)
            {
                hangDir = _inputs.Right() ? Vector3.forward : Vector3.back;
            }
            else // back
            {
                hangDir = _inputs.Right() ? Vector3.left : Vector3.right;
            }

            _slidingLeft = _inputs.Left(); // to rotate character late
            return hangDir;
        }
    }
}