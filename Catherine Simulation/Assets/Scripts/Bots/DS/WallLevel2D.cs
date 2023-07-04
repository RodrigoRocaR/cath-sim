using Bots.Algorithms;
using Bots.DS.MonteCarlo;
using LevelDS;
using UnityEngine;

namespace Bots.DS
{
    /**
     * Made to make some algorithms work easier with the level data by flattening the level into 2 dimensions
     * Each cell stores depth
     */
    public class WallLevel2D : Level2D
    {
        private const int LookUpDepthAfterBlockMove = 5;
        private readonly WallHelper _wallHelper;
        
        public WallLevel2D(Matrix3D<int> m) : base(m)
        {
        }

        public WallLevel2D(int[][] values) : base(values)
        {
        }

        public WallLevel2D() : base()
        {
        }

        public WallLevel2D(WallHelper wh)
        {
            _wallHelper = wh;
            TranslateTo2D();
        }
        
        public WallLevel2D(WallLevel2D prev, PushPullAction pushPullAction)
        {
            Elements = prev.GetWholeLevel(); // deep copy
            ModifyWithAction(pushPullAction);
        }

        private void ModifyWithAction(PushPullAction pushPullAction)
        {
            Vector3 blockPos = pushPullAction.BlockPos;
            PushPullAction.Actions a = pushPullAction.Action;
            (int i, int j, int k) = Level.TransformToIndexDomainAsTuple(blockPos);
            if (pushPullAction.IsAxisXMove())
            {
                // Check if it has blocks behind
                Elements[i, j] = GameConstants.EmptyBlock;
                for (int l = k; l < k+LookUpDepthAfterBlockMove; l++)
                {
                    if (Level.IsNotEmpty(i, j, l))
                    {
                        Elements[i, j] = l;
                    }
                }
            } 
            else if (pushPullAction.IsBackwardMove())
            {
                Elements[i, j]++;
            }
            else
            {
                Elements[i, j]--;
            }

        }
        
        /**
         *  Similar to translate to 2D but transforms a section of the level instead for performance reasons
         */
        private void TranslateTo2D()
        {
            for (int i = _wallHelper.GetStartX(); i < _wallHelper.GetStopX(); i++)
            {
                for (int j = _wallHelper.GetStartY(); j < _wallHelper.GetHeight(); j++)
                {
                    for (int k = 0; k < Level.Depth(); k++)
                    {
                        if (Level.IsNotEmpty(i, j, k))
                        {
                            Elements[i, j] = k;
                        }
                    }
                }
            }
        }
        
        
        /**
         *  (Recommended only for testing)
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