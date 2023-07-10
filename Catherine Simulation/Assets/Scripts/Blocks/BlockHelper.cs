using System;
using Bots.DS.MonteCarlo;
using UnityEngine;

namespace Blocks
{
    public class BlockHelper
    {
        private const int Offset = 1;

        public Vector3 Right(Vector3 pos, int multiplier = 1, int depthDelta = 0)
        {
            return new Vector3(pos.x + Offset * multiplier, pos.y, pos.z + depthDelta * Offset);
        }

        public Vector3 Left(Vector3 pos, int multiplier = 1, int depthDelta = 0)
        {
            return new Vector3(pos.x - Offset * multiplier, pos.y, pos.z + depthDelta * Offset);
        }

        public Vector3 Up(Vector3 pos, int multiplier = 1, int depthDelta = 0)
        {
            return new Vector3(pos.x, pos.y + Offset * multiplier,
                pos.z + depthDelta * Offset);
        }

        public Vector3 Down(Vector3 pos, int multiplier = 1, int depthDelta = 0)
        {
            return new Vector3(pos.x, pos.y - Offset * multiplier,
                pos.z + depthDelta * Offset);
        }

        public Vector3 Forward(Vector3 pos, int multiplier = 1)
        {
            return new Vector3(pos.x, pos.y, pos.z + Offset * multiplier);
        }

        public Vector3 Backward(Vector3 pos, int multiplier = 1)
        {
            return new Vector3(pos.x, pos.y, pos.z - Offset * multiplier);
        }

        public Vector3 TopRight(Vector3 pos, int depthDelta = 0)
        {
            return new Vector3(pos.x + Offset, pos.y + Offset,
                pos.z + depthDelta * Offset);
        }

        public Vector3 TopLeft(Vector3 pos, int depthDelta = 0)
        {
            return new Vector3(pos.x - Offset, pos.y + Offset,
                pos.z + depthDelta * Offset);
        }

        public Vector3 TopForward(Vector3 pos)
        {
            return new Vector3(pos.x, pos.y + Offset,
                pos.z + Offset);
        }

        public Vector3 BottomRight(Vector3 pos, int depthDelta = 0)
        {
            return new Vector3(pos.x + Offset, pos.y - Offset,
                pos.z + depthDelta * Offset);
        }

        public Vector3 BottomLeft(Vector3 pos, int depthDelta = 0)
        {
            return new Vector3(pos.x - Offset, pos.y - Offset,
                pos.z + depthDelta * Offset);
        }

        public static Vector3 GetNewBlockPos(Vector3 blockPos, PushPullAction a)
        {
            Vector3 newPos;
            switch (a.Action)
            {
                case PushPullAction.Actions.PushForward:
                    newPos = new Vector3(blockPos.x, blockPos.y, blockPos.z + Offset);
                    break;
                case PushPullAction.Actions.PushBackward:
                    newPos = new Vector3(blockPos.x, blockPos.y, blockPos.z - Offset);
                    break;
                case PushPullAction.Actions.PushRight:
                    newPos = new Vector3(blockPos.x + Offset, blockPos.y, blockPos.z);
                    break;
                case PushPullAction.Actions.PushLeft:
                    newPos = new Vector3(blockPos.x - Offset, blockPos.y, blockPos.z);
                    break;
                case PushPullAction.Actions.PullForward:
                    newPos = new Vector3(blockPos.x, blockPos.y, blockPos.z - Offset);
                    break;
                case PushPullAction.Actions.PullBackward:
                    newPos = new Vector3(blockPos.x, blockPos.y, blockPos.z + Offset);
                    break;
                case PushPullAction.Actions.PullRight:
                    newPos = new Vector3(blockPos.x - Offset, blockPos.y, blockPos.z);
                    break;
                case PushPullAction.Actions.PullLeft:
                    newPos = new Vector3(blockPos.x + Offset, blockPos.y, blockPos.z);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return newPos;
        }

        public static Vector3 GetNewPlayerPos(PushPullAction action)
        {
            Vector3 oldBlockPos = action.BlockPos;
            oldBlockPos.y -= Offset; // we are interested on the block below since it is used to feed the frontier
            Vector3 newPos;
            switch (action.Action)
            {
                case PushPullAction.Actions.PullForward:
                    newPos = new Vector3(oldBlockPos.x, oldBlockPos.y, oldBlockPos.z - 2*Offset);
                    break;
                case PushPullAction.Actions.PullRight:
                    newPos = new Vector3(oldBlockPos.x + 2*Offset, oldBlockPos.y, oldBlockPos.z);
                    break;
                case PushPullAction.Actions.PullBackward:
                    newPos = new Vector3(oldBlockPos.x, oldBlockPos.y, oldBlockPos.z + 2*Offset);
                    break;
                case PushPullAction.Actions.PullLeft:
                    newPos = new Vector3(oldBlockPos.x - 2*Offset, oldBlockPos.y, oldBlockPos.z);
                    break;
                case PushPullAction.Actions.PushForward:
                    newPos = new Vector3(oldBlockPos.x, oldBlockPos.y, oldBlockPos.z - Offset);
                    break;
                case PushPullAction.Actions.PushRight:
                    newPos = new Vector3(oldBlockPos.x + Offset, oldBlockPos.y, oldBlockPos.z);
                    break;
                case PushPullAction.Actions.PushBackward:
                    newPos = new Vector3(oldBlockPos.x, oldBlockPos.y, oldBlockPos.z + Offset);
                    break;
                case PushPullAction.Actions.PushLeft:
                    newPos = new Vector3(oldBlockPos.x - Offset, oldBlockPos.y, oldBlockPos.z);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return newPos;
        }
    }
}