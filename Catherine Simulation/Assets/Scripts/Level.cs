using UnityEngine;

public static class Level
{

    private static readonly LevelGenerator LevelGenerator = new LevelGenerator();
    public static readonly int[,,] LevelInt;
    public static readonly float BlockScale = 2;
    public static readonly Vector3 BlockScaleVector = new Vector3(2, 2, 2);
    
    static Level()
    {
        LevelInt = LevelGenerator.GetTestLevel();
    }
}
