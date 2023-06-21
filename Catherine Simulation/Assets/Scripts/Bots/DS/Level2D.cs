using LevelDS;

namespace Bots.DS
{
    /**
     * Made to make some algorithms work easier with the level data by flattening the level into 2 dimensions
     * Each cell stores height
     */
    public class Level2D
    {
        private Matrix2D<int> _level2D;

        public Level2D(Matrix3D<int> m)
        {
            _level2D = new Matrix2D<int>(m.Depth, m.Width);
            TranslateTo2D(m);
        }

        public Level2D(int[][] values)
        {
            // for tests
            _level2D = new Matrix2D<int>(values);
        }

        public Level2D()
        {
            // for tests
            _level2D = new Matrix2D<int>(new int[][] { });
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
                            _level2D[i, k] = j;
                        }
                    }
                }
            }
        }

        public int Width()
        {
            return _level2D.GetWidth();
        }

        public int Height()
        {
            return _level2D.GetHeight();
        }

        public int Get(int x, int y)
        {
            return _level2D[x, y];
        }
    }
}