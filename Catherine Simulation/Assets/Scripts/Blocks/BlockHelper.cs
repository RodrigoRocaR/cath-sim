using LevelDS;
using UnityEngine;

namespace Blocks
{
    public class BlockHelper
    {
        private int _offset;
        
        public BlockHelper()
        {
            _offset = Level.IsMock() ? 1 : GameConstants.BlockScale;
        }

        public Vector3 Right(Vector3 pos, int depthDelta = 0)
        {
            return new Vector3(pos.x + _offset, pos.y, pos.z + depthDelta * _offset);
        }

        public Vector3 Left(Vector3 pos, int depthDelta = 0)
        {
            return new Vector3(pos.x - _offset, pos.y, pos.z + depthDelta * _offset);
        }

        public Vector3 Up(Vector3 pos, int multiplier = 1, int depthDelta = 0)
        {
            return new Vector3(pos.x, pos.y + _offset * multiplier,
                pos.z + depthDelta * _offset);
        }

        public Vector3 Down(Vector3 pos, int multiplier = 1, int depthDelta = 0)
        {
            return new Vector3(pos.x, pos.y - _offset * multiplier,
                pos.z + depthDelta * _offset);
        }

        public Vector3 Forward(Vector3 pos)
        {
            return new Vector3(pos.x, pos.y, pos.z + _offset);
        }

        public Vector3 Backward(Vector3 pos)
        {
            return new Vector3(pos.x, pos.y, pos.z - _offset);
        }

        public Vector3 TopRight(Vector3 pos, int depthDelta = 0)
        {
            return new Vector3(pos.x + _offset, pos.y + _offset,
                pos.z + depthDelta * _offset);
        }

        public Vector3 TopLeft(Vector3 pos, int depthDelta = 0)
        {
            return new Vector3(pos.x - _offset, pos.y + _offset,
                pos.z + depthDelta * _offset);
        }

        public Vector3 BottomRight(Vector3 pos, int depthDelta = 0)
        {
            return new Vector3(pos.x + _offset, pos.y - _offset,
                pos.z + depthDelta * _offset);
        }

        public Vector3 BottomLeft(Vector3 pos, int depthDelta = 0)
        {
            return new Vector3(pos.x - _offset, pos.y - _offset,
                pos.z + depthDelta * _offset);
        }
    }
}