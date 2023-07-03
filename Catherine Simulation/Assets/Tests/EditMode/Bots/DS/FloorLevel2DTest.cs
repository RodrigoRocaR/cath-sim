using System.Collections.Generic;
using Bots.DS;
using LevelDS;
using NUnit.Framework;

namespace Tests.EditMode.Bots.DS
{
    public class FloorLevel2DTest
    {

        [Test]
        public void TestInitialize()
        {
            // Arrange
            Matrix3D<int> empty = new Matrix3D<int>(3, 4, 5);
            
            // Act
            Level2D l2d = new FloorLevel2D(empty);

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
            Dictionary<Matrix3D<int>, FloorLevel2D> levels = new Dictionary<Matrix3D<int>, FloorLevel2D>
            {
                {
                    new Matrix3D<int>( // like setting up walls upside down from left to right
                        new[]
                        {
                            new[] // x: 0
                            {
                                new[] { -1, 0, -1 }, // y: 0
                                new[] { -1, -1, 0 }, // y: 1
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
                    new FloorLevel2D(new[] // similar to a bird's eye view of the level
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
                    new FloorLevel2D(new[] // similar to a bird's eye view of the level
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
                FloorLevel2D l2d = new FloorLevel2D(levelPair.Key);

                // Assert
                Assert.True(l2d.AreLevel2DEqual(levelPair.Value));
            }
        }

        [Test]
        public void TestGet()
        {
            // This test might seem dumb but there could be problems when parsing coordinates from one system to another
            Level2D l2d = new FloorLevel2D(new[]
            {
                new[] { 0, 0, 0 },
                new[] { 0, 0, 3 },
                new[] { 0, 0, 0 }
            });
            
            Assert.AreEqual(3, l2d.Get(1, 2));
        }
    }
}