namespace LevelDS.LevelGen
{
    public class LevelBuilder
    {
        private int _levelSize;
        private GameMatrix _level;
        
        public LevelBuilder(int levelSize)
        {
            _levelSize = levelSize;
            _level = new GameMatrix(_levelSize, _levelSize, _levelSize);
            InitializeLevel();
        }

        public GameMatrix Build()
        {
            return _level;
        }
        
        private void InitializeLevel()
        {
            for (int i=0; i<_levelSize; i++)
            {
                for (int j=0; j<_levelSize; j++)
                {
                    for (int k=0; k<_levelSize; k++)
                    {
                        _level.SetBlockInt(i, j, k, GameConstants.EmptyBlock);
                    }
                }
            }
        }

        public LevelBuilder AddPlatform(int y)
        {
            for (int j=0; j<_levelSize; j++)
            {
                for (int k=0; k<_levelSize; k++)
                {
                    _level.SetBlockInt(j, y, k, GameConstants.SolidBlock);
                }
            }

            return this;
        }

        public LevelBuilder AddWall(int y,  int pos, int height = 2, bool horizontal = true)
        {
            for (int h=0; h<height; h++)
            {
                for (int k=0; k<_levelSize; k++)
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