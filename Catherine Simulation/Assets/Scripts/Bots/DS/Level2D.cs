using LevelDS;

namespace Bots.DS
{
    public abstract class Level2D
    {
        protected Matrix2D<int> Elements;

        protected Level2D(Matrix3D<int> m)
        {
            Elements = new Matrix2D<int>(m.Depth, m.Width);
            TranslateTo2D(m);
        }

        protected Level2D(int[][] values)
        {
            // for tests
            Elements = new Matrix2D<int>(values);
        }

        protected Level2D()
        {
            // for tests
            Elements = new Matrix2D<int>(new int[][] { });
        }
        
        protected abstract void TranslateTo2D(Matrix3D<int> m);

        public int Width()
        {
            return Elements.GetWidth();
        }

        public int Height()
        {
            return Elements.GetHeight();
        }

        public int Get(int x, int y)
        {
            return Elements[x, y];
        }
        
        public int Get((int, int) xy)
        {
            return Elements[xy.Item1, xy.Item2];
        }

        protected Matrix2D<int> GetWholeLevel()
        {
            return new Matrix2D<int>(Elements.GetClonedElements());
        }

        public bool AreLevel2DEqual(Level2D l2)
        {
            if (Width() != l2.Width() || Height() != l2.Height())
            {
                return false;
            }

            for (int i = 0; i < Width(); i++)
            {
                for (int j = 0; j < Width(); j++)
                {
                    if (Get(i, j) != l2.Get(i, j))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}