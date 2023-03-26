
using UnityEngine;

public class LevelGenerator 
{
    // This stores the level as integers
    // The dimensions are [y, x, z]
    private int[,,] _level = new int[Level.LevelSize, Level.LevelSize, Level.LevelSize];
    
    private int[,,] _levelPredefined1 =
    {
        {
            {0, 0, 0},
            {0, 0, 0},
            {0, 0, 0}
        },
        {
            {0, -1, -1},
            {0, 0, 0},
            {-1, -1, -1}
        },
        {
            {-1, -1, -1},
            {-1, 0, -1},
            {-1, -1, -1}
        },
    };

    public int[,,] GetPredefinedLevel()
    {
        return _levelPredefined1;
    }
    
    
    public int[,,] GetTestLevel()
    {
        InitializeLevel();
        AddPlatform(0);
        AddWall(1, Level.LevelSize-1, 3);
        AddWall(1, Level.LevelSize-1, 3, false);
        AddWall(1, Level.LevelSize-5, 1, false);
        AddWall(1, 0, 3, false);
        _level[3, 7, 1] = 0;
        return _level;
    }

    private void InitializeLevel()
    {
        for (int i=0; i<Level.LevelSize; i++)
        {
            for (int j=0; j<Level.LevelSize; j++)
            {
                for (int k=0; k<Level.LevelSize; k++)
                {
                    _level[i, j, k] = Level.EmptyBlock;
                }
            }
        }
    }


    private void AddPlatform(int y)
    {
        for (int j=0; j<Level.LevelSize; j++)
        {
            for (int k=0; k<Level.LevelSize; k++)
            {
                _level[y, j, k] = Level.SolidBlock;
            }
        }
    }

    private void AddWall(int y,  int pos, int height = 2, bool horizontal = true)
    {
        for (int h=0; h<height; h++)
        {
            for (int k=0; k<Level.LevelSize; k++)
            {
                if (horizontal)
                {
                    _level[y + h, pos, k] = Level.SolidBlock;
                }
                else
                {
                    _level[y + h, k, pos] = Level.SolidBlock;
                }
            }
        }
    }
}
