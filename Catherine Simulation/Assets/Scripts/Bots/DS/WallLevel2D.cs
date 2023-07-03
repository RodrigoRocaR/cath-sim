using LevelDS;

namespace Bots.DS
{
    /**
     * Made to make some algorithms work easier with the level data by flattening the level into 2 dimensions
     * Each cell stores depth
     */
    public class WallLevel2D : Level2D
    {
        public WallLevel2D(Matrix3D<int> m) : base(m)
        {
        }

        public WallLevel2D(int[][] values) : base(values)
        {
        }

        public WallLevel2D() : base()
        {
        }
        
        /**
         *  The x and y values should correlate
         *  Integers represent absolute depth coords (world coordinates) instead of the type of the block
         */
        protected override void TranslateTo2D(Matrix3D<int> m)
        {
            for (int i = 0; i < m.Width; i++)
            {
                for (int j = 0; j < m.Height; j++)
                {
                    for (int k = 0; k < m.Depth; k++)
                    {
                        if (m[i, j, k] != GameConstants.EmptyBlock)
                        {
                            Elements[i, j] = k;
                        }
                    }
                }
            }
        }
    }
}