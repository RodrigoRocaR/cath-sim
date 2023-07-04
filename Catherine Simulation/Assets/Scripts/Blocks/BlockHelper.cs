using System;
using Bots.DS.MonteCarlo;
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

        public Vector3 Right(Vector3 pos, int multiplier = 1, int depthDelta = 0)
        {
            return new Vector3(pos.x + _offset * multiplier, pos.y, pos.z + depthDelta * _offset);
        }

        public Vector3 Left(Vector3 pos, int multiplier = 1, int depthDelta = 0)
        {
            return new Vector3(pos.x - _offset * multiplier, pos.y, pos.z + depthDelta * _offset);
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

        public Vector3 Forward(Vector3 pos, int multiplier = 1)
        {
            return new Vector3(pos.x, pos.y, pos.z + _offset * multiplier);
        }

        public Vector3 Backward(Vector3 pos, int multiplier = 1)
        {
            return new Vector3(pos.x, pos.y, pos.z - _offset * multiplier);
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

        public static Vector3 GetNewBlockPos(Vector3 blockPos, PushPullAction a)
        {
            Vector3 newPos;
            switch (a.Action)
            {
                case PushPullAction.Actions.PushForward:
                    newPos = new Vector3(blockPos.x, blockPos.y, blockPos.z + GameConstants.BlockScale);
                    break;
                case PushPullAction.Actions.PushBackward:
                    newPos = new Vector3(blockPos.x, blockPos.y, blockPos.z - GameConstants.BlockScale);
                    break;
                case PushPullAction.Actions.PushRight:
                    newPos = new Vector3(blockPos.x + GameConstants.BlockScale, blockPos.y, blockPos.z);
                    break;
                case PushPullAction.Actions.PushLeft:
                    newPos = new Vector3(blockPos.x - GameConstants.BlockScale, blockPos.y, blockPos.z);
                    break;
                case PushPullAction.Actions.PullForward:
                    newPos = new Vector3(blockPos.x, blockPos.y, blockPos.z - GameConstants.BlockScale);
                    break;
                case PushPullAction.Actions.PullBackward:
                    newPos = new Vector3(blockPos.x, blockPos.y, blockPos.z + GameConstants.BlockScale);
                    break;
                case PushPullAction.Actions.PullRight:
                    newPos = new Vector3(blockPos.x - GameConstants.BlockScale, blockPos.y, blockPos.z);
                    break;
                case PushPullAction.Actions.PullLeft:
                    newPos = new Vector3(blockPos.x + GameConstants.BlockScale, blockPos.y, blockPos.z);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return newPos;
        }
        
        public static Vector3 GetNewPlayerPos(Vector3 oldPos, PushPullAction action)
        {
            if (action.IsPushAction()) return oldPos;

            Vector3 newPos;
            switch (action.Action)
            {
                case PushPullAction.Actions.PullForward:
                    newPos = new Vector3(oldPos.x, oldPos.y, oldPos.z - GameConstants.BlockScale);
                    break;
                case PushPullAction.Actions.PullRight:
                    newPos = new Vector3(oldPos.x + GameConstants.BlockScale, oldPos.y, oldPos.z);
                    break;
                case PushPullAction.Actions.PullBackward:
                    newPos = new Vector3(oldPos.x, oldPos.y, oldPos.z + GameConstants.BlockScale);
                    break;
                case PushPullAction.Actions.PullLeft:
                    newPos = new Vector3(oldPos.x - GameConstants.BlockScale, oldPos.y, oldPos.z);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return newPos;
        }
    }
}