namespace LevelDS.LevelGen
{
    public class LevelFactory
    {
        private readonly int _levelSize;

        public LevelFactory(int levelSize)
        {
            _levelSize = levelSize;
        }
        
        public GameMatrix GetTestLevel()
        {
            return new LevelBuilder(_levelSize)
                .AddPlatform(0)
                .AddWall(1, _levelSize - 1, 3)
                .AddWall(1, _levelSize - 1, 3, false)
                .AddWall(1, _levelSize - 5, 1, false)
                .AddWall(1, 0, 3, false)
                .AddIndividualBlock(1, 1, 3, GameConstants.SolidBlock)
                .Build();
        }
    }
}