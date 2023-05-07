namespace LevelDS.LevelGen
{
    public class LevelBuilder
    {
        private readonly int _levelSizeX;
        private readonly int _levelSizeY;
        private readonly int _levelSizeZ;
        
        private GameMatrix _level;
        
        public LevelBuilder(int levelSizeX, int levelSizeY, int levelSizeZ)
        {
            _levelSizeX = levelSizeX;
            _levelSizeY = levelSizeY;
            _levelSizeZ = levelSizeZ;
            _level = new GameMatrix(_levelSizeX, _levelSizeY, _levelSizeZ);
            InitializeLevel();
        }

        public GameMatrix Build()
        {
            return _level;
        }
        
        private void InitializeLevel()
        {
            for (int i=0; i<_levelSizeX; i++)
            {
                for (int j=0; j<_levelSizeY; j++)
                {
                    for (int k=0; k<_levelSizeZ; k++)
                    {
                        _level.SetBlockInt(i, j, k, GameConstants.EmptyBlock);
                    }
                }
            }
        }

        public LevelBuilder AddPlatform(int y)
        {
            for (int j=0; j<_levelSizeX; j++)
            {
                for (int k=0; k<_levelSizeZ; k++)
                {
                    _level.SetBlockInt(j, y, k, GameConstants.SolidBlock);
                }
            }

            return this;
        }

        public LevelBuilder AddWall(int y,  int pos, int height = 2, bool horizontal = true)
        {
            int wallLength = horizontal ? _levelSizeX : _levelSizeZ;
            for (int h=0; h<height; h++)
            {
                for (int k=0; k<wallLength; k++)
                {
                    if (horizontal)
                    {
                        _level.SetBlockInt(k, y+h, pos, GameConstants.SolidBlock);
                    }
                    else
                    {
                        _level.SetBlockInt(pos, y+h, k, GameConstants.SolidBlock);
                    }
                }
            }

            return this;
        }

        public LevelBuilder AddIndividualBlock(int x, int y, int z, int blockType)
        {
            _level.SetBlockInt(x, y, z, blockType);
            return this;
        }
    }
}