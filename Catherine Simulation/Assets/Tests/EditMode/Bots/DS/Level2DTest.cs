using System.Collections.Generic;
using Bots.DS;
using LevelDS;
using NUnit.Framework;

namespace Tests.EditMode.Bots.DS
{
    public class Level2DTest
    {
        private bool AreLevel2DEqual(Level2D l1, Level2D l2)
        {
            if (l1.Width() != l2.Width() || l1.Height() != l2.Height())
            {
                return false;
            }

            for (int i = 0; i < l1.Width(); i++)
            {
                for (int j = 0; j < l1.Width(); j++)
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
            Assert.AreEqual(5, l2d.Width()); // depth
            Assert.AreEqual(3, l2d.Height()); // width
            for (int i=0; i<l2d.Width(); i++)
            {
                for (int j = 0; j < l2d.Height(); j++)
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
                        new[] { 2, 2, 2 }, // x: 0
                        new[] { 1, 1, 1 }, // x: 1
                        new[] { 0, 0, 0 } // x: 2
                    })
                },
                {
                    new Matrix3D<int>(
                        new[]
                        {
                            new[] // x: 0
                            {
                                new[] { -1, -1, 0 }, // y: 0
                                new[] { -1, -1, 0 }, // y: 1
                                new[] { -1, -1, 0 } // y: 2
                            },
                            new[] // x: 1
                            {
                                new[] { -1, -1, -1 },
                                new[] { -1, -1, -1 },
                                new[] { -1, -1, -1 }
                            },
                            new[] // x: 2
                            {
                                new[] { -1, -1, -1 },
                                new[] { -1, -1, -1 },
                                new[] { -1, -1, -1 }
                            },
                        }),
                    new Level2D(new[] // similar to a bird's eye view of the level
                    {
                        new[] { -1, -1, 2 }, // x: 0
                        new[] { -1, -1, -1 }, // x: 1
                        new[] { -1, -1, -1 } // x: 2
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

        [Test]
        public void TestGet()
        {
            // This test might seem dumb but there could be problems when parsing coordinates from one system to another
            Level2D l2d = new Level2D(new[]
            {
                new[] { 0, 0, 0 },
                new[] { 0, 0, 3 },
                new[] { 0, 0, 0 }
            });
            
            Assert.AreEqual(3, l2d.Get(1, 2));
        }
    }
}