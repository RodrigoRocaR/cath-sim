using LevelDS;

namespace Bots.DS
{
    /**
     * Made to make some algorithms work easier with the level data by flattening the level into 2 dimensions
     * Each cell stores height
     */
    public class FloorLevel2D : Level2D
    {
        public FloorLevel2D(Matrix3D<int> m) : base(m)
        {
        }

        public FloorLevel2D(int[][] values) : base(values)
        {
        }

        public FloorLevel2D() : base()
        {
        }
        
        /**
         *  The y in 2D represents the z value in the level
         *  The x value correlates
         *  Integers represent absolute height coords (world coordinates) instead of the type of the block
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
                            Elements[i, k] = j;
                        }
                    }
                }
            }
        }
    }
}