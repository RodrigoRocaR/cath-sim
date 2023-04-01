using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMatrix
{
    private Matrix3D<int> _levelInt;
    private Matrix3D<GameObject> _level;

    private GameObject _parentGameObject;
    
    public int Width { get; set; }

    public int Height { get; set; }

    public int Depth { get; set; }

    public GameMatrix(int width, int height, int depth, GameObject parentGameObject)
    {
        Width = width;
        Height = height;
        Depth = depth;
        _levelInt = new Matrix3D<int>(width, height, depth);
        _level = new Matrix3D<GameObject>(width, height, depth);
        _parentGameObject = parentGameObject;
        InitializeMatrices();
    }
    
    public int GetBlockInt(int x, int y, int z)
    {
        return _levelInt == null ? -1 : _levelInt[x, y, z];
    }
    
    public int GetBlockInt(Vector3 coord)
    {
        if (_levelInt == null) return -1;
        
        coord.y -= 1;
        coord /= Level.BlockScale;
        return IsCoordWithinLevel(coord.x, coord.y, coord.z) ? _levelInt[coord] : -1;
    }

    public void SetBlockInt(int x, int y, int z, int val)
    {
        if (!IsCoordWithinLevelInt(x, y, z)) return;
        _levelInt[x, y, z] = val;
    }

    public void SetBlock(int x, int y, int z, GameObject block)
    {
        _level[x, y, z] = block;
        block.transform.SetParent(_parentGameObject.transform);
    }
    
    private bool IsCoordWithinLevel(float x, float y, float z)
    {
        return IsCoordWithinLevelInt((int)Math.Round(x), (int)Math.Round(y), (int)Math.Round(z));
    }
    
    private bool IsCoordWithinLevelInt(int x, int y, int z)
    {
        return x < Width && x >= 0 && y < Height && y >= 0 && z < Depth && z >= 0;
    }
    

    public bool isEmpty()
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
}
