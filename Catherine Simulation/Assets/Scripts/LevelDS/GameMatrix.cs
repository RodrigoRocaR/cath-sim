using System;
using UnityEngine;

namespace LevelDS
{
    public class GameMatrix
    {
        private readonly GameObject _emptyGameObject = new GameObject();
        
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
            _levelInt = new Matrix3D<int>(width, height, depth);
            _level = new Matrix3D<GameObject>(width, height, depth);
            InitializeMatrices();
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
            if (!IsCoordWithinLevelInt(x, y, z)) return;
            _levelInt[x, y, z] = val;
        }
        
        // Access GameObject matrix --------------
        public void SetBlock(int x, int y, int z, GameObject block)
        {
            _level[x, y, z] = block;
        }
        
        public GameObject GetBlock(Vector3 coord)
        {
            if (_levelInt == null) return _emptyGameObject;

            coord = ParseCoords(coord);
            return IsCoordWithinLevel(coord) ? _level[coord] : _emptyGameObject;
        }
    
        // Helper functions ------------------------------
        private bool IsCoordWithinLevel(Vector3 coord)
        {
            return IsCoordWithinLevelInt((int)Math.Round(coord.x), (int)Math.Round(coord.y), (int)Math.Round(coord.z));
        }
    
        private bool IsCoordWithinLevelInt(int x, int y, int z)
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

        private void InitializeMatrices()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    for (int k = 0; k < Depth; k++)
                    {
                        _levelInt[i, j, k] = Level.EmptyBlock;
                    }
                }
            }
        }

        private Vector3 ParseCoords(Vector3 coords)
        {
            coords.y -= 1;
            coords /= Level.BlockScale;
            return coords;
        }
    }
}
