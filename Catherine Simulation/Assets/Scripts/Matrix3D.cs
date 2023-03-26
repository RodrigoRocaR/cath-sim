using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix3D
{
    private int[,,] _blocks;
    private int width; // x
    private int height; // y
    private int depth; // z

    public Matrix3D(int width, int height, int depth) {
        this.width = width;
        this.height = height;
        this.depth = depth;
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
