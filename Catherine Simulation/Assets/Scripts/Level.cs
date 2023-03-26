using UnityEngine;

public static class Level
{
    public const int EmptyBlock = -1;
    public const int SolidBlock = 0;
    public const int LevelSize = 12;

    private static readonly LevelGenerator LevelGenerator = new LevelGenerator();
    public static readonly int[,,] LevelInt;
    public static readonly float BlockScale = 2;
    public static readonly Vector3 BlockScaleVector = new Vector3(2, 2, 2);
    
    static Level()
    {
        LevelInt = LevelGenerator.GetTestLevel();
    }

    public static bool IsCoordWithinLevel(Vector3 coords)
    {
        return coords.x/BlockScale < Level.LevelSize && 
               coords.y/BlockScale < Level.LevelSize && 
               coords.z/BlockScale < Level.LevelSize;
    }

    public static int GetBlock(Vector3 coord)
    {
        return LevelInt[(int) (coord.y/BlockScale), (int) (coord.x/BlockScale), (int) (coord.z/BlockScale)];
    }
}
