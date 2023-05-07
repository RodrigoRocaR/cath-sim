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
            for (int i = 0; i < _levelSizeX; i++)
            {
                for (int j = 0; j < _levelSizeY; j++)
                {
                    for (int k = 0; k < _levelSizeZ; k++)
                    {
                        _level.SetBlockInt(i, j, k, GameConstants.EmptyBlock);
                    }
                }
            }
        }

        public LevelBuilder AddPlatform(int y)
        {
            for (int j = 0; j < _levelSizeX; j++)
            {
                for (int k = 0; k < _levelSizeZ; k++)
                {
                    _level.SetBlockInt(j, y, k, GameConstants.SolidBlock);
                }
            }

            return this;
        }

        public LevelBuilder AddWall(int y, int pos, int height = 2, bool horizontal = true)
        {
            int wallLength = horizontal ? _levelSizeX : _levelSizeZ;
            for (int h = 0; h < height; h++)
            {
                for (int k = 0; k < wallLength; k++)
                {
                    if (horizontal)
                    {
                        _level.SetBlockInt(k, y + h, pos, GameConstants.SolidBlock);
                    }
                    else
                    {
                        _level.SetBlockInt(pos, y + h, k, GameConstants.SolidBlock);
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

        public LevelBuilder AddBlockRowX(int x, int y)
        {
            AddWall(y, x, 1);
            return this;
        }

        public LevelBuilder AddBlockRowZ(int z, int y)
        {
            AddWall(y, z, 1, false);
            return this;
        }

        public LevelBuilder AddSquareBorder(int y, int size, int startX = 0, int startZ = 0)
        {
            int sizeX = size + startX < _levelSizeX ? size : _levelSizeX;
            int sizeZ = size + startZ < _levelSizeZ ? size : _levelSizeZ;
            for (int i = startX; i < sizeX; i++)
            {
                for (int k = startZ; k < sizeZ; k++)
                {
                    if (i == startX || i == sizeX - 1 || k == startZ || k == sizeZ - 1)
                        _level.SetBlockInt(i, y, k, GameConstants.SolidBlock);
                }
            }

            return this;
        }
        
    }
}