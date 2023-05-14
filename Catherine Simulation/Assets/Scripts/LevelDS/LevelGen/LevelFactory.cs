namespace LevelDS.LevelGen
{
    public class LevelFactory
    {

        public GameMatrix GetTestLevel()
        {
            int levelSize = 12;
            return new LevelBuilder(levelSize, levelSize, levelSize)
                .AddPlatform(0)
                .AddWall(1, levelSize - 1, 3) // wall at the front
                .AddWall(1, levelSize - 1, 3, false) // wall at the right
                .AddWall(1, levelSize - 5, 1, false) // wall at the left
                .AddWall(1, 0, 3, false)
                .AddIndividualBlock(1, 1, 3, GameConstants.SolidBlock)
                .AddIndividualBlock(6, 1, 6, GameConstants.VictoryBlock)
                .Build();
        }

        public GameMatrix GetTestHangingLevel()
        {
            int levelSize = 11;
            return new LevelBuilder(levelSize, levelSize, levelSize)
                .AddSquareBorder(0, levelSize)
                .AddBlockRowX(5, 0)
                .AddBlockRowZ(5, 0)
                .AddPyramid2D(1, 5, 10, 4)
                .Build();
        }
    }
}