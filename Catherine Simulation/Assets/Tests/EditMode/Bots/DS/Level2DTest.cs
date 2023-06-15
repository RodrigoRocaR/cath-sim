using System.Collections.Generic;
using Bots.DS;
using LevelDS;
using NUnit.Framework;

namespace Tests.EditMode.LevelDS
{
    public class Level2DTest
    {
        private bool AreLevel2DEqual(Level2D l1, Level2D l2)
        {
            var dims1 = l1.GetSize();
            var dims2 = l2.GetSize();
            if (dims1[0] != dims2[0] || dims1[1] != dims2[0])
            {
                return false;
            }

            for (int i = 0; i < dims1[0]; i++)
            {
                for (int j = 0; j < dims2[0]; j++)
                {
                    if (l1.Get(i, j) != l2.Get(i, j))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        [Test]
        public void TestInitialize()
        {
            // Arrange
            Matrix3D<int> empty = new Matrix3D<int>(3, 4, 5);
            
            // Act
            Level2D l2d = new Level2D(empty);

            // Assert
            var size = l2d.GetSize();
            Assert.AreEqual(5, size[0]); // depth
            Assert.AreEqual(3, size[1]); // width
            for (int i=0; i<size[0]; i++)
            {
                for (int j = 0; j < size[1]; j++)
                {
                    Assert.AreEqual(GameConstants.EmptyBlock, l2d.Get(i,j));
                }
            }
        }

        [Test]
        public void TestTranslateTo2D()
        {
            // Arrange
            Dictionary<Matrix3D<int>, Level2D> levels = new Dictionary<Matrix3D<int>, Level2D>
            {
                {
                    new Matrix3D<int>( // like setting up walls upside down from left to right
                        new[]
                        {
                            new[] // x: 0
                            {
                                new[] { -1, -1, -1 }, // y: 0
                                new[] { -1, -1, -1 }, // y: 1
                                new[] { 0, 0, 0 } // y: 2
                            },
                            new[] // x: 1
                            {
                                new[] { -1, -1, -1 },
                                new[] { 0, 0, 0 },
                                new[] { -1, -1, -1 }
                            },
                            new[] // x: 2
                            {
                                new[] { 0, 0, 0 },
                                new[] { -1, -1, -1 },
                                new[] { -1, -1, -1 }
                            },
                        }),
                    new Level2D(new[] // similar to a bird's eye view of the level
                    {
                        new[] { 2, 1, 0 }, // z: 0
                        new[] { 2, 1, 0 }, // z: 1
                        new[] { 2, 1, 0 } // z: 2
                    })
                }
            };

            foreach (var levelPair in levels)
            {
                // Act
                Level2D l2d = new Level2D(levelPair.Key);

                // Assert
                Assert.True(AreLevel2DEqual(l2d, levelPair.Value));
            }
        }
    }
}