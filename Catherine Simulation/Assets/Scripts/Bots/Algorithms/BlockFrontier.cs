using System;
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
        private readonly GameMatrix _currentLevel;
        private List<Vector3> _interestPoints;

        private int _wallZValue;

        public BlockFrontier(Vector3 playerPos, GameMatrix currentLevel)
        {
            _currentLevel = currentLevel;
            _bh = new BlockHelper();
            _frontier = new HashSet<Vector3>();
            _interestPoints = new List<Vector3> { playerPos };
            GetFrontierAlgorithm();
        }

        public BlockFrontier(HashSet<Vector3> frontier)
        {
            // for testing
            _frontier = frontier;
            _bh = new BlockHelper();
        }

        private void GetFrontierAlgorithm(int i = 0)
        { 
            GetFrontierFromPos(_interestPoints[i]);
            if (i+1 < _interestPoints.Count) GetFrontierAlgorithm(i + 1);
        }

        private void GetFrontierFromPos(Vector3 pos)
        {
            bool finished = false;
            bool exploringRight = true;
            bool iterationNotOfInterest = false;
            Vector3 currBlock = pos;

            GoForwardUntilWall();
            MoveUntilCanGetOnBlock(currBlock);
            currBlock = NextBlock();
            while (!finished)
            {
                if (_currentLevel.IsNotEmpty(_bh.Up(currBlock))) // Case 1: block in the way
                {
                    AddBlock(_bh.Up(currBlock));
                    if (_currentLevel.IsNotEmpty(_bh.Up(currBlock, multiplier: 2))) // 1.A) Wall
                    {
                        // We try to hang before giving up
                        if (!CanHang()) FinishExploration();
                        currBlock = NextBlock(); // first available block
                        Hang();
                    }
                    else // 1.B) Just one block
                    {
                        currBlock = _bh.Up(currBlock);
                        AddBlock(_bh.Up(currBlock, depthDelta: 1));
                    }
                }
                else if (_currentLevel.IsNotEmpty(currBlock)) // Case 2: Flat terrain
                {
                    AddBlock(_bh.Up(currBlock, depthDelta: 1));
                    AddBlock(_bh.Up(currBlock, depthDelta: -1));
                }
                else if (_currentLevel.IsNotEmpty(_bh.Down(currBlock))) // Case 3: block 1 level below
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
                currBlock = PrevBlock();
                if (!_interestPoints.Contains(currBlock) && !iterationNotOfInterest) _interestPoints.Add(currBlock);
                if (exploringRight)
                {
                    exploringRight = false;
                    currBlock = pos;
                    MoveUntilCanGetOnBlock(pos);
                }
                else finished = true;

                iterationNotOfInterest = false;
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
                return _currentLevel.IsNotEmpty(currBlock) && _currentLevel.IsNotEmpty(NextBlock());
            }

            bool CanNotGetUp()
            {
                return _currentLevel.IsNotEmpty(_bh.Up(currBlock)) ||
                       _currentLevel.IsNotEmpty(_bh.Up(currBlock, multiplier: 2));
            }

            void Hang(bool exploreMode = true)
            {
                bool dontRunOutOfBlocks = HangHorizontally();

                // If on exploreMode, after this we plan to continue iterating until we have to set the block 
                // to the block previous to the available one (or empty one and hence finish exploring)
                if (exploreMode) currBlock = PrevBlock();
                if (dontRunOutOfBlocks)
                {
                    if (!exploreMode)
                    {
                        if (exploringRight) AddBlock(_bh.TopLeft(currBlock));
                        else AddBlock(_bh.TopRight(currBlock));
                    }
                    else AddBlock(_bh.Up(currBlock)); // wall to the left will not be added otherwise when width >= 2
                }
                else
                { // ran out of blocks, we would add empty blocks infinitely
                    iterationNotOfInterest = true;
                }
            }

            bool HangHorizontally()
            {
                bool dontRunOutOfBlocks = _currentLevel.IsNotEmpty(currBlock);
                while (dontRunOutOfBlocks && CanNotGetUp())
                {
                    currBlock = NextBlock();
                    // There are blocks in the way, so we have to hang on them instead: (hang backwards)
                    while (_currentLevel.IsNotEmpty(_bh.Backward(currBlock))) currBlock = _bh.Backward(currBlock);
                    // We dont have more blocks to hang, try to hang forward
                    while (_currentLevel.IsEmpty(currBlock) && _currentLevel.IsNotEmpty(PrevBlock()))
                        currBlock = _bh.Forward(currBlock);

                    dontRunOutOfBlocks = _currentLevel.IsNotEmpty(currBlock);
                }

                return dontRunOutOfBlocks;
            }

            void MoveUntilCanGetOnBlock(Vector3 p)
            {
                if (_currentLevel.IsNotEmpty(p))
                {
                    AddInitialBlock();
                    return;
                }

                currBlock = GetHangingBlock(p);
                if (!CanNotGetUp()) // Can get up, no need to hang
                {
                    AddInitialBlock();
                    return;
                }

                Hang(exploreMode: false);
                if (_currentLevel.IsEmpty(currBlock) && exploringRight) // We ran out of blocks --> go to the left
                {
                    currBlock = _bh.Forward(pos);
                    exploringRight = false;
                    Hang(exploreMode: false);
                    if (_currentLevel.IsEmpty(currBlock)) // Ran out of blocks again
                        finished = true;
                }
                else if (_currentLevel.IsEmpty(currBlock) && !exploringRight) // Going to the left and ran out of blocks
                {
                    finished = true;
                    return;
                }

                AddInitialBlock();
            }
            
            Vector3 GetHangingBlock(Vector3 p)
            {
                
                if (_currentLevel.IsNotEmpty(_bh.Forward(p)))
                {
                    return _bh.Forward(p);
                }
                if (_currentLevel.IsNotEmpty(_bh.Left(p)))
                {
                    return _bh.Left(p);
                }
                if (_currentLevel.IsNotEmpty(_bh.Right(p)))
                {
                    return _bh.Right(p);
                }
                if (_currentLevel.IsNotEmpty(_bh.Backward(p)))
                {
                    return _bh.Backward(p);
                }
                Debug.LogWarning("I am hanging in the void");
                return p;
            }

            void AddInitialBlock()
            {
                AddBlock(_bh.Up(currBlock, depthDelta: 1));
            }

            void GoForwardUntilWall()
            {
                bool reachedWall = false;
                while (!reachedWall)
                {
                    if (CanGoForward(currBlock))
                    {
                        currBlock = _bh.Forward(currBlock);
                    }
                    else if (_currentLevel.IsNotEmpty(currBlock) && CanGoForward(_bh.Up(currBlock)))
                    {
                        // go up by 1 (cannot do this if hanging)
                        currBlock = _bh.TopForward(currBlock);
                    }
                    else
                    {
                        reachedWall = true;
                    }
                }

                _wallZValue = Mathf.Max((int)currBlock.z+1, _wallZValue);
            }

            bool CanGoForward(Vector3 p)
            {
                return _currentLevel.IsNotEmpty(_bh.Forward(p)) &&
                       _currentLevel.IsEmpty(_bh.Forward(_bh.Up(p))) &&
                       _currentLevel.IsEmpty(_bh.TopForward(_bh.Up(p)));
            }
        }

        private void AddBlock(Vector3 blockPos)
        {
            if (_currentLevel.IsNotEmpty(blockPos))
            {
                _frontier.Add(blockPos);
            }
        }

        public HashSet<Vector3> GetFrontier()
        {
            return _frontier;
        }

        public bool ContainsBlocksOfNextWall()
        {
            foreach (var blockPos in _frontier)
            {
                if ((int)blockPos.z > _wallZValue) return true;
            }

            return false;
        }

        public int Length()
        {
            return _frontier.Count;
        }
    }
}