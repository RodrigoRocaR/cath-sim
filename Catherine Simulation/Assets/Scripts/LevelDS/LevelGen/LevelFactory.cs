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
                .Build();
        }

        public GameMatrix GetTestHangingLevel()
        {
            int levelSize = 10;
            return new LevelBuilder(levelSize, levelSize, levelSize)
                .AddSquareBorder(0, levelSize)
                .AddBlockRowX(5, 0)
                .AddBlockRowZ(5, 0)
                .Build();
        }
    }
}