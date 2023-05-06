using Tools;
using UnityEngine;
using LevelDS;

namespace Player.Controllers
{
    public class HangController
    {
        private const float HangingDistancePercentageToBlock = 1.5f;

        private readonly Inputs _inputs;
        private readonly Transform _transform;
        private readonly PlayerState _playerState;
        private readonly Rigidbody _rigidbody;

        private MultiMoveLerp _multiMoveLerp;

        public HangController(Transform transform, PlayerState playerState, Inputs inputs, Rigidbody rigidbody)
        {
            _transform = transform;
            _inputs = inputs;
            _playerState = playerState;
            _rigidbody = rigidbody;
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
                RotateToFaceBlock();
            }
            else if (_playerState.IsHangingOnBorder() && _inputs.Horizontal()) // sliding left / right
            {
                SetupHangingSlide();
            }
        }

        private void SetupHangingSlide()
        {
            Vector3 direction = _inputs.Right() ? Vector3.right : Vector3.left;
            Vector3 playerLookingDirection = _playerState.GetDirection();
            Vector3 playerPos = _transform.position;
            Vector3 targetPos = playerPos + direction * Level.BlockScale;
            Vector3 checkPos = PlayerPos2BlockInFrontPos(playerPos);

            Debug.Log(checkPos);

            bool isBlockBehind = Level.IsBlock(checkPos + playerLookingDirection * Level.BlockScale);
            checkPos += direction * Level.BlockScale;
            bool isBlockSameLevel = Level.IsBlock(checkPos);
            bool isBlockInTheWay = Level.IsBlock(checkPos - playerLookingDirection * Level.BlockScale);

            if (isBlockSameLevel && !isBlockInTheWay) // Slide horizontally on the same wall
            {
                _multiMoveLerp = new MultiMoveLerp(
                    new[] { 0.75f },
                    new[] { playerPos, targetPos }
                );
                return;
            }

            // Corner case (literally)
            Vector3 midPos = playerPos + direction * (Level.BlockScale / 2f * HangingDistancePercentageToBlock);

            if (isBlockInTheWay) // corner backward
            {
                targetPos = midPos - _playerState.GetDirection() * Level.BlockScale / 2;
                _multiMoveLerp = new MultiMoveLerp(
                    new[] { 0.33f, 0.32f },
                    new[]
                    {
                        playerPos,
                        midPos,
                        targetPos
                    }
                );
                return;
            }

            if (isBlockBehind) // corner forward
            {
                targetPos = midPos + _playerState.GetDirection() * Level.BlockScale / 2;
                _multiMoveLerp = new MultiMoveLerp(
                    new[] { 0.33f, 0.32f },
                    new[]
                    {
                        playerPos,
                        midPos,
                        targetPos
                    }
                );
            }
        }


        private void SetupDropOnBorder()
        {
            FixDirection();

            Vector3 playerPos = _transform.position;

            Vector3 blockEdge = playerPos + _playerState.GetDirection() *
                ((Level.BlockScale / 2f) * HangingDistancePercentageToBlock);
            Vector3 hangingPos = blockEdge;
            hangingPos.y -= Level.BlockScale / (0.75f * Level.BlockScale);

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

        private void RotateToFaceBlock()
        {
            _transform.Rotate(new Vector3(0, 180, 0));
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
            Vector3 actualDirection = (_playerState.GetTarget() - _transform.position) / Level.BlockScale;
            if (_playerState.GetDirection() != actualDirection)
            {
                _playerState.SetDirection(actualDirection);
            }
        }

        private Vector3 PlayerPos2BlockInFrontPos(Vector3 hangingPos)
        {
            Vector3 direction = _playerState.GetDirection();
            // Fix horizontal coords
            float horizontalFix = (Level.BlockScale / 2f) * HangingDistancePercentageToBlock;
            if (direction == Vector3.forward)
            {
                hangingPos.z += horizontalFix;
            }
            else if (direction == Vector3.back)
            {
                hangingPos.z -= horizontalFix;
            }
            else if (direction == Vector3.right)
            {
                hangingPos.x += horizontalFix;
            }
            else if (direction == Vector3.left)
            {
                hangingPos.x -= horizontalFix;
            }

            // Fix vertical coords
            hangingPos.y +=  Level.BlockScale/2f - (Level.BlockScale / (0.75f * Level.BlockScale))/2;

            return hangingPos;
        }
    }
}