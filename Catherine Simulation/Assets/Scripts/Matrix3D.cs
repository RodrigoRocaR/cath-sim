using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix3D
{
    private int[,,] _blocks;
    public int Width { get; }

    public int Height { get; }

    public int Depth { get; }


    public Matrix3D(int width, int height, int depth) {
        this.Width = width;
        this.Height = height;
        this.Depth = depth;
        this._blocks = new int[width, height, depth];
    }

    public int this[int x, int y, int z] {
        get => _blocks[x, y, z];
        set => _blocks[x, y, z] = value;
    }
    
    public int this[Vector3Int coord] {
        get => _blocks[coord.x, coord.y, coord.z];
        set => _blocks[coord.x, coord.y, coord.z] = value;
    }
    
    public int this[Vector3 coord] {
        get
        {
            Vector3Int coordInt = Vector3Int.RoundToInt(coord);
            return _blocks[coordInt.x, coordInt.y, coordInt.z];
        }

        set
        {
            Vector3Int coordInt = Vector3Int.RoundToInt(coord);
            _blocks[coordInt.x, coordInt.y, coordInt.z] = value;
        }
    }
}
