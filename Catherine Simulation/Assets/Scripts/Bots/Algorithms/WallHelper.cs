using LevelDS;
using UnityEngine;

namespace Bots.Algorithms
{
    /**
     * Miscellaneous logic related to climbing a wall
     * Works in the index domain
     */
    public class WallHelper
    {
        private const int LookUpLeftWallRangeMargin = 10;

        private int _height;
        private int _targetZ;
        private Vector3 _playerPos;
        private (int, int) _totalWallRangeX;

        public WallHelper(Vector3 playerPos)
        {
            _targetZ = (int)playerPos.z + 1;
            _playerPos = playerPos;
            _totalWallRangeX = (0, 0);
            GetWallHeight();
        }

        private void GetWallHeight()
        {
            int startY = (int)_playerPos.y;
            var wallRange = GetWallRange((int)_playerPos.x, startY);


            int threshold = (wallRange.Item2-wallRange.Item1) / 2;
            int holes = 0;
            
            int minimumHighestRowWithHoles = -1;
            bool previousRowHadHoles = false;
            bool thisRowHadHoles = false;

            for (int j = startY + 1; j < Level.Height(); j++)
            {
                for (int i = wallRange.Item1; i <= wallRange.Item2; i++)
                {
                    if (Level.IsEmpty(i, j, _targetZ) && Level.IsEmpty(i, j+1, _targetZ))
                    {
                        holes++;
                        if (!previousRowHadHoles)
                        {
                            minimumHighestRowWithHoles = j;
                        }

                        thisRowHadHoles = true;
                    }
                    
                    if (holes >= threshold)
                    {
                        int h = minimumHighestRowWithHoles == -1 ? j : minimumHighestRowWithHoles;
                        _height = h - startY;
                        return;
                    }
                }

                previousRowHadHoles = thisRowHadHoles;
                thisRowHadHoles = false;
                holes = 0;

                wallRange = GetWallRange(wallRange.Item1, j + 1);
                threshold = (wallRange.Item2-wallRange.Item1) / 2;
            }
            int toph = minimumHighestRowWithHoles == -1 ? Level.Width() : minimumHighestRowWithHoles;
            _height = toph - startY;
        }

        private (int, int) GetWallRange(int startX, int j)
        {
            (int, int) wallRange = (0, 0);
            bool chainStarted = false;
            for (int i = startX - LookUpLeftWallRangeMargin; i < Level.Width(); i++)
            {
                if (Level.IsNotEmpty(i, j, _targetZ) && !chainStarted)
                {
                    wallRange.Item1 = i;
                    chainStarted = true;

                    if (_totalWallRangeX.Item1 < i) _totalWallRangeX.Item1 = i;
                }

                if (Level.IsEmpty(i, j, _targetZ) && chainStarted)
                {
                    wallRange.Item2 = i;
                    
                    if (_totalWallRangeX.Item2 < i) _totalWallRangeX.Item2 = i;
                    
                    return wallRange;
                }
            }

            wallRange.Item2 = Level.Width() - 1;
            if (_totalWallRangeX.Item2 < wallRange.Item2) _totalWallRangeX.Item2 = wallRange.Item2;
            return wallRange;
        }

        public int GetHeight()
        {
            return (int)_playerPos.y + _height;
        }
        
        public int GetRelativeHeight()
        {
            return _height;
        }

        public int GetTargetZ()
        {
            return _targetZ;
        }

        public int GetStartX()
        {
            return _totalWallRangeX.Item1;
        }

        public int GetStopX()
        {
            return _totalWallRangeX.Item2 + 1;
        }

        public int GetStartY()
        {
            return (int)_playerPos.y;
        }
    }
}