namespace LevelDS
{
    /**
     * Made to make some algorithms work easier with the level data by flattening the level into 2 dimensions
     */
    public class Level2D
    {
        private int[][] _level2D;

        public Level2D(Matrix3D<int> m)
        {
            Initialize(m.Depth, m.Width);
            TranslateTo2D(m);
        }

        public Level2D(int[][] values)
        {
            // for tests
            _level2D = values;
        }

        /**
         *  The y in 2D represents the z value in the level
         *  The x value correlates
         *  Integers represent absolute height coords (world coordinates) instead of the type of the block
         */
        private void TranslateTo2D(Matrix3D<int> m)
        {
            for (int i = 0; i < m.Width; i++)
            {
                for (int j = 0; j < m.Height; j++)
                {
                    for (int k = 0; k < m.Depth; k++)
                    {
                        if (m[i, j, k] != GameConstants.EmptyBlock)
                        {
                            _level2D[k][i] = j;
                        }
                    }
                }
            }
        }

        private void Initialize(int depth, int width)
        {
            _level2D = new int[depth][];
            for (int i = 0; i < depth; i++)
            {
                _level2D[i] = new int[width];
                for (int j = 0; j < width; j++)
                {
                    _level2D[i][j] = GameConstants.EmptyBlock;
                }
            }
        }

        public int[] GetSize()
        {
            return new int[] { _level2D.Length, _level2D[0].Length };
        }

        public int Get(int x, int y)
        {
            return _level2D[x][y];
        }
    }
}