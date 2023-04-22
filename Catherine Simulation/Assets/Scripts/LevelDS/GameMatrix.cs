using System;
using UnityEngine;

namespace LevelDS
{
    public class GameMatrix
    {

        private Matrix3D<int> _levelInt;
        private Matrix3D<GameObject> _level;

        public int Width { get; set; }

        public int Height { get; set; }

        public int Depth { get; set; }

        public GameMatrix(int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
            _levelInt = new Matrix3D<int>(width, height, depth, Level.EmptyBlock);
            _level = new Matrix3D<GameObject>(width, height, depth, null);
        }
    
        // Access integer matrix --------------
        public int GetBlockInt(int x, int y, int z)
        {
            return _levelInt == null ? -1 : _levelInt[x, y, z];
        }
    
        public int GetBlockInt(Vector3 coord)
        {
            if (_levelInt == null) return -1;

            coord = ParseCoords(coord);
            return IsCoordWithinLevel(coord) ? _levelInt[coord] : -1;
        }

        public void SetBlockInt(int x, int y, int z, int val)
        {
            if (!IsCoordWithinLevel(x, y, z))
            {
                IncreaseSize(x, y, z);
                Vector3 coord = ClampToZero(x, y, z);
                _levelInt[coord] = val;
            }
            else
            {
                _levelInt[x, y, z] = val;
            }
        }
        
        public void SetBlockInt(Vector3 coord, int val)
        {
            coord = ParseCoords(coord);
            if (!IsCoordWithinLevel(coord))
            {
                IncreaseSize(coord);
                coord = ClampToZero(coord);
            }
            _levelInt[coord] = val;
        }
        
        
        // Access GameObject matrix --------------
        public void SetBlock(int x, int y, int z, GameObject block)
        {
            if (!IsCoordWithinLevel(x, y, z))
            {
                IncreaseSize(x, y, z);
                Vector3 coord = ClampToZero(x, y, z);
                _level[coord] = block;
            }
            else
            {
                _level[x, y, z] = block;
            }
        }
        
        public void SetBlock(Vector3 coord, GameObject block)
        {
            coord = ParseCoords(coord);
            if (!IsCoordWithinLevel(coord))
            {
                IncreaseSize(coord);
                coord = ClampToZero(coord);
            }

            _level[coord] = block;
        }

        public GameObject GetBlock(Vector3 coord)
        {
            if (_levelInt == null) return null;

            coord = ParseCoords(coord);
            return IsCoordWithinLevel(coord) ? _level[coord] : null;
        }
        
        public GameObject GetBlock(int x, int y, int z)
        {
            if (_levelInt == null) return null;
            
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
                        if (_levelInt[i, j, k] != Level.EmptyBlock) return false;
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

            Width += diffx >= 0 ? diffx : -diffx;
            Height += diffy >= 0 ? diffy : -diffy;;
            Depth += diffz >= 0 ? diffz : -diffz;;
        }

        private Vector3 ParseCoords(Vector3 coords)
        {
            coords.y -= 1;
            coords /= Level.BlockScale;
            return coords;
        }
        
        private Vector3 ClampToZero(Vector3 vector)
        {
            return new Vector3(Mathf.Max(0, vector.x), Mathf.Max(0, vector.y), Mathf.Max(0, vector.z));
        }
        
        private Vector3 ClampToZero(int x, int y, int z)
        {
            return new Vector3(Mathf.Max(0, x), Mathf.Max(0, y), Mathf.Max(0, z));
        }
    }
}
