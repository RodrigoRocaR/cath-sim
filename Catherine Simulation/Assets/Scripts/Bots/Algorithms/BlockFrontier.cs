using System.Collections.Generic;
using Blocks;
using LevelDS;
using UnityEngine;

namespace Bots.Algorithms
{
    public class BlockFrontier
    {
        private HashSet<Vector3> _frontier;
        private BlockHelper _bh;

        public BlockFrontier(Vector3 playerPos)
        {
            _bh = new BlockHelper();
            _frontier = new HashSet<Vector3>();
            GetFrontierFromPos(playerPos); // todo: check if this is the block the player is above at (stepping on)
        }

        private void GetFrontierFromPos(Vector3 pos)
        {
            AddBlock(_bh.Up(pos, depthDelta:1));

            bool finished = false;
            bool exploringRight = true;
            Vector3 currBlock = _bh.Right(pos);
            while (!finished)
            {
                if (Level.IsNotEmpty(_bh.Up(currBlock)))
                {
                    currBlock = _bh.Up(currBlock);
                    AddBlock(currBlock); // the block in the way
                    if (Level.IsNotEmpty(_bh.Up(currBlock)))
                    { // this is a wall, cannot continue
                        if (exploringRight)
                        {
                            exploringRight = false;
                            currBlock = pos;
                        }
                        else finished = true;
                    }
                    else
                    {
                        AddBlock(_bh.Up(currBlock, depthDelta: 1)); // the one in the wall above the single block
                    }
                }
                else if (Level.IsNotEmpty(currBlock))
                {
                    AddBlock(_bh.Up(currBlock, depthDelta: 1)); // ground level
                }
                else if (Level.IsNotEmpty(_bh.Down(currBlock)))
                {
                    AddBlock(_bh.Forward(currBlock));
                    AddBlock(_bh.Left(currBlock));
                    currBlock = _bh.Down(currBlock);
                }
                else
                {
                    if (exploringRight)
                    {
                        exploringRight = false;
                        currBlock = pos;
                    }
                    else finished = true;
                }

                if (exploringRight)
                {
                    currBlock = _bh.Right(currBlock);
                }
                else
                {
                    currBlock = _bh.Left(currBlock);
                }
            }
        }

        private void AddBlock(Vector3 blockPos)
        {
            if (Level.IsNotEmpty(blockPos))
            {
                _frontier.Add(blockPos);
            }
        }

        public HashSet<Vector3> GetFrontier()
        {
            return _frontier;
        }
    }
}