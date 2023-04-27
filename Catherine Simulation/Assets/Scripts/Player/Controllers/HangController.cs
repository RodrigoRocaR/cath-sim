using Tools;
using UnityEngine;
using LevelDS;

namespace Player.Controllers
{
    public class HangController
    {
        private const float HangingDistanceToBlock = 0.1f;

        private readonly Inputs _inputs;
        private readonly Transform _transform;
        private readonly PlayerState _playerState;

        private MultiMoveLerp<MoveLerp> _multiMoveLerp;

        public HangController(Transform transform, Inputs inputs, PlayerState playerState)
        {
            _transform = transform;
            _inputs = inputs;
            _playerState = playerState;
        }

        // Entry point
        public void Hang()
        {
            if (_playerState.IsPerformingHangingAction())
            {
                if (_multiMoveLerp.IsCompleted())
                {
                    _playerState.StopHangAction();
                    _multiMoveLerp = null;
                }
                else
                {
                    _multiMoveLerp.Lerp();
                }
            }
            else if (_playerState.CanHangOnBorder())
            {
                // TODO: check on state to switch between 3 functions
                setupDropOnBorder();
            }
        }

        /*
         * 1. Center of block to edge of block
         * 2. Go 1 block below + distance to wall in horizontal
         */
        private void setupDropOnBorder()
        {
            Vector3 playerPos = _transform.position;
            Vector3 blockEdge = playerPos + _playerState.GetDirection() * Level.BlockScale / 2;
            Vector3 hangingPos = V3Calc.AddNum(
                V3Calc.SubstractNum(blockEdge, Level.BlockScale / 2),
                HangingDistanceToBlock);

            _playerState.DropOnBorder();
            _multiMoveLerp = new MultiMoveLerp<MoveLerp>(
                new[] { 1f, 2f },
                new[]
                {
                    playerPos,
                    blockEdge,
                    hangingPos
                });
        }
    }
}