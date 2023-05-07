using Tools;
using UnityEngine;
using LevelDS;

namespace Player.Controllers
{
    public class HangController
    {
        private const float DistancePercentageToBlock = 1.5f;

        private const float HorizontalOffset =
            GameConstants.BlockScale / 2f * DistancePercentageToBlock;

        private const float VerticalOffset = GameConstants.BlockScale / (0.75f * GameConstants.BlockScale);

        private const float DistanceCorneringFromMidPosToTarget = GameConstants.BlockScale * 0.75f;

        private readonly Inputs _inputs;
        private readonly Transform _transform;
        private readonly PlayerState _playerState;
        private readonly Rigidbody _rigidbody;
        private readonly CameraTiled _cameraTiled;

        private MultiMoveLerp _multiMoveLerp;
        private bool _slidingLeft;

        public HangController(Transform transform, PlayerState playerState, Inputs inputs, Rigidbody rigidbody,
            CameraTiled cameraTiled)
        {
            _transform = transform;
            _inputs = inputs;
            _playerState = playerState;
            _rigidbody = rigidbody;
            _cameraTiled = cameraTiled;
        }

        // Entry point
        public void Hang()
        {
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
            else if (_playerState.CanDropOnBorder())
            {
                // TODO: check on state to switch between 3 functions
                SetupDropOnBorder();
                _rigidbody.useGravity = false;
                RotateToFaceBlockOnDrop();
            }
            else if (_playerState.IsHangingOnBorder() && _inputs.Horizontal()) // sliding left / right
            {
                SetupHangingSlide();
            }
        }

        private void SetupHangingSlide()
        {
            Vector3 targetHangDirection = GetTargetHangDirection();
            Vector3 playerLookingDirection = _playerState.GetDirection();
            Vector3 playerPos = _transform.position;
            Vector3 targetPos = playerPos + targetHangDirection * GameConstants.BlockScale;
            Vector3 checkPos = PlayerPos2BlockInFrontPos(playerPos);


            bool isBlockBehind = Level.IsBlock(checkPos + playerLookingDirection * GameConstants.BlockScale);
            checkPos += targetHangDirection * GameConstants.BlockScale;
            bool isBlockSameLevel = Level.IsBlock(checkPos);
            bool isBlockInTheWay = Level.IsBlock(checkPos - playerLookingDirection * GameConstants.BlockScale);

            if (isBlockSameLevel && !isBlockInTheWay) // Slide horizontally on the same wall
            {
                _multiMoveLerp = new MultiMoveLerp(
                    new[] { 0.75f },
                    new[] { playerPos, targetPos }
                );
                return;
            }

            // Corner case (literally)
            Vector3 midPos = playerPos + targetHangDirection * HorizontalOffset;

            if (isBlockInTheWay) // corner backward
            {
                targetPos = midPos - _playerState.GetDirection() * DistanceCorneringFromMidPosToTarget;
                _multiMoveLerp = new MultiMoveLerp(
                    new[] { 0.33f, 0.32f },
                    new[]
                    {
                        playerPos,
                        midPos,
                        targetPos
                    }
                );
                RotateToFaceBlockCorner(false);
                _cameraTiled.RotateCamera(_playerState.GetDirection());
                return;
            }

            if (isBlockBehind) // corner forward
            {
                targetPos = midPos + _playerState.GetDirection() * DistanceCorneringFromMidPosToTarget;
                _multiMoveLerp = new MultiMoveLerp(
                    new[] { 0.33f, 0.32f },
                    new[]
                    {
                        playerPos,
                        midPos,
                        targetPos
                    }
                );
                RotateToFaceBlockCorner(true);
                _cameraTiled.RotateCamera(_playerState.GetDirection());
            }
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
                new[] { 0.8f, 0.4f },
                new[]
                {
                    playerPos,
                    blockEdge,
                    hangingPos
                });
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
                _playerState.StopDroppingOnBorder();
                _playerState.HangOnBorder();
            }
            else if (_playerState.IsGettingUpFromBorder()) // ended getting up
            {
                _playerState.StopGettingUpFromBorder();
                _playerState.StopHanging();
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