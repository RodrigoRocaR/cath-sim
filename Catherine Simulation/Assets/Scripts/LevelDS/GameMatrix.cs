using System;
using Blocks;
using Blocks.BlockTypes;
using Bots.DS.MonteCarlo;
using UnityEngine;

namespace LevelDS
{
    public class GameMatrix
    {

        private Matrix3D<int> _levelInt;
        private Matrix3D<IBlock> _level;
        private Vector3Int _negativeExpanding;

        private bool _isMock;

        public int Width { get; set; }

        public int Height { get; set; }

        public int Depth { get; set; }

        public GameMatrix(int width, int height, int depth, bool isMock = false)
        {
            Width = width;
            Height = height;
            Depth = depth;
            _levelInt = new Matrix3D<int>(Width, Height, Depth, GameConstants.EmptyBlock);
            _level = new Matrix3D<IBlock>(Width, Height, Depth, null);
            _negativeExpanding = new Vector3Int(0, 0, 0);
            _isMock = isMock;
        }
        
        public GameMatrix(GameMatrix prev, PushPullAction action)
        {
            Width = prev.Width;
            Height = prev.Height;
            Depth = prev.Depth;
            _levelInt = prev._levelInt.GetDeepCopy();
            _negativeExpanding = new Vector3Int(0, 0, 0);
            _isMock = false;
            ModifyWithAction(action);
        }
        
        private void ModifyWithAction(PushPullAction pushPullAction)
        {
            Vector3 blockPos = pushPullAction.BlockPos;
            int type = _levelInt[blockPos];
            _levelInt[blockPos] = GameConstants.EmptyBlock;
            _levelInt[BlockHelper.GetNewBlockPos(blockPos, pushPullAction)] = type;
        }
    
        // Access integer matrix --------------
        public int GetBlockInt(int x, int y, int z)
        {
            if (_levelInt == null) return -1;

            (x, y, z) = AdaptNegativeCoords(x, y, z);
            return IsCoordWithinLevel(x, y, z) ? _levelInt[x, y, z] : -1;
        }
    
        public int GetBlockInt(Vector3 coord)
        {
            if (_levelInt == null) return -1;
            
            coord = ParseCoords(coord);
            coord = AdaptNegativeCoords(coord);
            return IsCoordWithinLevel(coord) ? _levelInt[coord] : -1;
        }

        public void SetBlockInt(int x, int y, int z, int val)
        {
            (x, y, z) = AdaptNegativeCoords(x, y, z);
            if (!IsCoordWithinLevel(x, y, z))
            {
                IncreaseSize(x, y, z);
                (x, y, z) = AdaptNegativeCoords(x, y, z);
            }
            _levelInt[x, y, z] = val;
        }
        
        public void SetBlockInt(Vector3 coord, int val)
        {
            coord = ParseCoords(coord);
            coord = AdaptNegativeCoords(coord);
            if (!IsCoordWithinLevel(coord))
            {
                IncreaseSize(coord);
                coord = AdaptNegativeCoords(coord);
            }
            _levelInt[coord] = val;
        }

        public bool IsEmpty(int i, int j, int k)
        {
            return _levelInt[i, j, k] == GameConstants.EmptyBlock;
        }
        
        public bool IsNotEmpty(int i, int j, int k)
        {
            return !IsEmpty();
        }
        
        public bool IsEmpty(Vector3 pos)
        {
            return !IsCoordWithinLevel(pos) || _levelInt[pos] == GameConstants.EmptyBlock;
        }

        public bool IsNotEmpty(Vector3 pos)
        {
            return !IsEmpty(pos);
        }

        public bool CanBeMoved(Vector3 pos)
        {
            return IsCoordWithinLevel(pos) && GameConstants.MovableBlocks.Contains(_levelInt[pos]);
        }

        public bool IsVictoryBlock(Vector3 pos)
        {
            return IsCoordWithinLevel(pos) && _levelInt[pos] == GameConstants.VictoryBlock;
        }
        
        
        // Access GameObject matrix --------------
        public void SetBlock(int x, int y, int z, IBlock block)
        {
            (x, y, z) = AdaptNegativeCoords(x, y, z);
            if (!IsCoordWithinLevel(x, y, z))
            {
                IncreaseSize(x, y, z);
                (x, y, z) = AdaptNegativeCoords(x, y, z);
            }
            _level[x, y, z] = block;
        }
        
        public void SetBlock(Vector3 coord, IBlock block)
        {
            coord = ParseCoords(coord);
            coord = AdaptNegativeCoords(coord);
            if (!IsCoordWithinLevel(coord))
            {
                IncreaseSize(coord);
                coord = AdaptNegativeCoords(coord);
            }

            _level[coord] = block;
        }

        public IBlock GetBlock(Vector3 coord)
        {
            if (_levelInt == null) return null;
            
            coord = ParseCoords(coord);
            coord = AdaptNegativeCoords(coord);
            return IsCoordWithinLevel(coord) ? _level[coord] : null;
        }
        
        public IBlock GetBlock(int x, int y, int z)
        {
            if (_level == null) return null;
            (x, y, z) = AdaptNegativeCoords(x, y, z);
            
            return IsCoordWithinLevel(x, y, z) ? _level[x, y, z] : null;
        }
    
        // Helper functions ------------------------------
        private bool IsCoordWithinLevel(Vector3 coord)
        {
            return IsCoordWithinLevel((int)Math.Round(coord.x), (int)Math.Round(coord.y), (int)Math.Round(coord.z));
        }
    
        private bool IsCoordWithinLevel(int x, int y, int z)
        {
            return x < Width && x >= 0 && y < Height && y >= 0 && z < Depth && z >= 0;
        }
    

        public bool IsEmpty()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    for (int k = 0; k < Depth; k++)
                    {
                        if (_levelInt[i, j, k] != GameConstants.EmptyBlock) return false;
                    }
                }
            }
            return true;
        }

        private void IncreaseSize(Vector3 coord)
        {
            IncreaseSize((int)Math.Round(coord.x), (int)Math.Round(coord.y), (int)Math.Round(coord.z));
        }
        
        private void IncreaseSize(int x, int y, int z)
        {
            int diffx = x > 0 ? Math.Max(0, x - _levelInt.Width + 1) : x;
            int diffy = y > 0 ? Math.Max(0, y - _levelInt.Height + 1) : y;
            int diffz = z > 0 ? Math.Max(0, z - _levelInt.Depth + 1) : z;
            
            _levelInt.IncreaseSize(0, diffx);
            _level.IncreaseSize(0, diffx);
            _levelInt.IncreaseSize(1, diffy);
            _level.IncreaseSize(1, diffy);
            _levelInt.IncreaseSize(2, diffz);
            _level.IncreaseSize(2, diffz);

            if (diffx >= 0) Width += diffx;
            else
            {
                Width -= diffx;
                _negativeExpanding.x -= diffx;
            }
            
            if (diffy >= 0) Height += diffy;
            else
            {
                Height -= diffy;
                _negativeExpanding.y -= diffy;
            }
            
            if (diffz >= 0) Depth += diffz;
            else
            {
                Depth -= diffz;
                _negativeExpanding.z -= diffz;
            }
            
        }

        private Vector3 ParseCoords(Vector3 coords)
        {
            if (_isMock) return coords;
            coords.y -= 1;
            coords /= GameConstants.BlockScale;
            return coords;
        }

        private Vector3 AdaptNegativeCoords(Vector3 coords)
        {
            return coords + _negativeExpanding;
        }
        
        private (int, int, int) AdaptNegativeCoords(int x, int y, int z)
        {
            return (x+_negativeExpanding.x, y+_negativeExpanding.y, z+_negativeExpanding.z);
        }

        public Matrix3D<int> GetMatrixInt()
        {
            return _levelInt;
        }
        
        public Matrix3D<IBlock> GetMatrixBlocks()
        {
            return _level;
        }
    }
}
