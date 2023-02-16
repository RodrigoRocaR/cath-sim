
public class LevelGenerator 
{
    private const int LevelSize = 12;
    private int[,,] _level = new int[LevelSize, LevelSize, LevelSize];
    
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
        AddWall(1, LevelSize-1, 3);
        AddWall(1, LevelSize-1, 3, false);
        AddWall(1, LevelSize-5, 1, false);
        AddWall(1, 0, 3, false);
        return _level;
    }

    private void InitializeLevel()
    {
        for (int i=0; i<LevelSize; i++)
        {
            for (int j=0; j<LevelSize; j++)
            {
                for (int k=0; k<LevelSize; k++)
                {
                    _level[i, j, k] = -1;
                }
            }
        }
    }


    private void AddPlatform(int y)
    {
        for (int j=0; j<LevelSize; j++)
        {
            for (int k=0; k<LevelSize; k++)
            {
                _level[y, j, k] = 0;
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
                    _level[y+h, pos, k] = 0;
                }
                else
                {
                    _level[y+h, k, pos] = 0;
                }
            }
        }
    }
}
