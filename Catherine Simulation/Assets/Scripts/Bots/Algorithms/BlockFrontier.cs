using System.Collections.Generic;
using Blocks;
using LevelDS;
using UnityEngine;

namespace Bots.Algorithms
{
    // todo: check that it works when player is hanging
    // todo: check that it works when there is a fall??
    // todo: add boolean flag when it is possible to add blocks of above wall (next Z?)
    public class BlockFrontier
    {
        private HashSet<Vector3> _frontier;
        private BlockHelper _bh;

        public BlockFrontier(Vector3 playerPos)
        {
            if (!Level.IsMock())
            {
                playerPos = Level.TransformToIndexDomain(playerPos);
            }

            _bh = new BlockHelper();
            _frontier = new HashSet<Vector3>();
            GetFrontierFromPos(playerPos); // todo: check if this is the block the player is above at (stepping on)
        }

        public BlockFrontier(HashSet<Vector3> frontier)
        {
            // for testing
            _frontier = frontier;
            _bh = new BlockHelper();
        }

        private void GetFrontierFromPos(Vector3 pos)
        {
            AddBlock(_bh.Up(pos, depthDelta: 1));

            bool finished = false;
            bool exploringRight = true;
            Vector3 currBlock = _bh.Right(pos);
            while (!finished)
            {
                if (Level.IsNotEmpty(_bh.Up(currBlock))) // Case 1: block in the way
                {
                    AddBlock(_bh.Up(currBlock));
                    if (Level.IsNotEmpty(_bh.Up(currBlock, multiplier: 2))) // 1.A) Wall
                    {
                        // We try to hang before giving up
                        if (!CanHang()) FinishExploration();
                        Hang();
                    }
                    else // 1.B) Just one block
                    {
                        currBlock = _bh.Up(currBlock);
                        AddBlock(_bh.Up(currBlock, depthDelta: 1));
                    }
                }
                else if (Level.IsNotEmpty(currBlock)) // Case 2: Flat terrain
                {
                    AddBlock(_bh.Up(currBlock, depthDelta: 1));
                    AddBlock(_bh.Up(currBlock, depthDelta: -1));
                }
                else if (Level.IsNotEmpty(_bh.Down(currBlock))) // Case 3: block 1 level below
                {
                    AddBlock(_bh.Forward(currBlock));
                    AddBlock(_bh.Left(currBlock));
                    currBlock = _bh.Down(currBlock);
                }
                else // We are in the middle of void
                {
                    FinishExploration();
                }

                currBlock = NextBlock();
            }

            void FinishExploration() // Local function to terminate exploring the current direction
            {
                if (exploringRight)
                {
                    exploringRight = false;
                    currBlock = pos;
                }
                else finished = true;
            }

            Vector3 NextBlock()
            {
                return exploringRight ? _bh.Right(currBlock) : _bh.Left(currBlock);
            }
            
            Vector3 PrevBlock()
            {
                return exploringRight ? _bh.Left(currBlock) : _bh.Right(currBlock);
            }

            bool CanHang()
            {
                return Level.IsNotEmpty(currBlock) && Level.IsNotEmpty(NextBlock());
            }

            void Hang()
            {
                currBlock = NextBlock(); // first available block
                bool dontRunOutOfBlocks = Level.IsNotEmpty(currBlock);
                while (dontRunOutOfBlocks && (Level.IsNotEmpty(_bh.Up(currBlock)) ||
                                              Level.IsNotEmpty(_bh.Up(currBlock, multiplier: 2))))
                {
                    // We can continue hanging but we can not get up to the block since it is blocked
                    currBlock = NextBlock();
                    dontRunOutOfBlocks = Level.IsNotEmpty(currBlock);
                }
                // Since after this call we plan to continue iterating we have to set the block to the block previous
                // to the available one (or empty one and hence finish exploring)
                currBlock = PrevBlock();
                if (dontRunOutOfBlocks) AddBlock(_bh.Up(currBlock)); // wall to the left will not be added otherwise when width >= 2
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