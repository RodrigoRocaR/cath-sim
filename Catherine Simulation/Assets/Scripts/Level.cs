using System;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject[] blockVariants;
    public const int EmptyBlock = -1;
    public const int SolidBlock = 0;
    public const int LevelSize = 12;
    public const int BlockScale = 2;
    
    public readonly Vector3 startCoords = new Vector3(0, 0.5f, 0);

    private static Matrix3D _level;

    void Start()
    {
        _level = new Matrix3D(LevelSize, LevelSize, LevelSize);
        InitializeLevel();
        GetTestLevel();
        SpawnBlocks();
    }

    void Update()
    {
        
    }

    private void SpawnBlocks()
    {
        for (int i=0; i<_level.Width; i++)
        {
            for (int j=0; j<_level.Height; j++)
            {
                for (int k=0; k<_level.Depth; k++)
                {
                    if (_level[i, j, k] != EmptyBlock)
                    {
                        Instantiate(blockVariants[_level[i, j, k]], 
                            new Vector3((startCoords.x+i)*BlockScale, 
                                (startCoords.y+j)*BlockScale, 
                                (startCoords.z+k)*BlockScale), 
                            blockVariants[_level[i, j, k]].transform.rotation);
                    }
                }
            }
        }
    }

    private void InitializeLevel()
    {
        for (int i=0; i<LevelSize; i++)
        {
            for (int j=0; j<LevelSize; j++)
            {
                for (int k=0; k<LevelSize; k++)
                {
                    _level[i, j, k] = EmptyBlock;
                }
            }
        }
    }

    private void GetTestLevel()
    {
        AddPlatform(0);
        AddWall(1, LevelSize-1, 3);
        AddWall(1, LevelSize-1, 3, false);
        AddWall(1, LevelSize-5, 1, false);
        AddWall(1, 0, 3, false);
        _level[1, 1, 3] = 0;
    }
    
    private void AddPlatform(int y)
    {
        for (int j=0; j<LevelSize; j++)
        {
            for (int k=0; k<LevelSize; k++)
            {
                _level[j, y, k] = SolidBlock;
            }
        }
    }

    private void AddWall(int y,  int pos, int height = 2, bool horizontal = true)
    {
        for (int h=0; h<height; h++)
        {
            for (int k=0; k<LevelSize; k++)
            {
                if (horizontal)
                {
                    _level[k, y + h, pos] = SolidBlock;
                }
                else
                {
                    _level[pos, y + h, k] = SolidBlock;
                }
            }
        }
    }
    
    public static bool IsCoordWithinLevel(int x, int y, int z)
    {
        return x is < LevelSize and >= 0 && y is < LevelSize and >= 0 && z is < LevelSize and >= 0;
    }
    public static bool IsCoordWithinLevel(float x, float y, float z)
    {
        return IsCoordWithinLevel((int)Math.Round(x), (int)Math.Round(y), (int)Math.Round(z));
    }

    public static int GetBlock(int x, int y, int z)
    {
        return _level[x, y, z];
    }
    
    public static int GetBlock(Vector3Int coord)
    {
        coord.y -= 1;
        coord /= BlockScale;
        return IsCoordWithinLevel(coord.x, coord.y, coord.z) ? _level[coord] : -1;
    }
    
    public static int GetBlock(Vector3 coord)
    {
        coord.y -= 1;
        coord /= BlockScale;
        return IsCoordWithinLevel(coord.x, coord.y, coord.z) ? _level[coord] : -1;
    }
}
