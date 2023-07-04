using System.Collections.Generic;
using Bots.DS;
using Bots.DS.MonteCarlo;
using LevelDS;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.Bots.DS
{
    public class WallLevel2DTest
    {
        [Test]
        public void TestTranslateTo2D()
        {
            // Arrange
            Dictionary<Matrix3D<int>, WallLevel2D> levels = new Dictionary<Matrix3D<int>, WallLevel2D>
            {
                {
                    new Matrix3D<int>( // like setting up walls upside down from left to right
                        new[]
                        {
                            new[] // x: 0
                            {
                                new[] { -1, 0, -1 }, // y: 0
                                new[] { -1, 0, -1 }, // y: 1
                                new[] { -1, 0, -1 } // y: 2
                            },
                            new[] // x: 1
                            {
                                new[] { -1, 0, -1 },
                                new[] { -1, 0, -1 },
                                new[] { -1, 0, -1 }
                            },
                            new[] // x: 2
                            {
                                new[] { -1, 0, -1 },
                                new[] { -1, 0, -1 },
                                new[] { -1, 0, -1 }
                            },
                        }),
                    new WallLevel2D(new[]
                    {
                        new[] { 1, 1, 1 }, // x: 0
                        new[] { 1, 1, 1 }, // x: 1
                        new[] { 1, 1, 1 } // x: 2
                    })
                },
                {
                    new Matrix3D<int>(
                        new[]
                        {
                            new[] // x: 0
                            {
                                new[] { -1, -1, -1 }, // y: 0
                                new[] { -1, -1, 0 }, // y: 1
                                new[] { -1, -1, 0 } // y: 2
                            },
                            new[] // x: 1
                            {
                                new[] { -1, 0, -1 },
                                new[] { -1, -1, 0 },
                                new[] { -1, -1, -1 }
                            },
                            new[] // x: 2
                            {
                                new[] { -1, -1, -1 },
                                new[] { -1, -1, -1 },
                                new[] { 0, -1, -1 }
                            },
                        }),
                    new WallLevel2D(new[]
                    {
                        new[] { -1, 2, 2 }, // x: 0
                        new[] { 1, 2, -1 }, // x: 1
                        new[] { -1, -1, 0 } // x: 2
                    })
                }
            };

            foreach (var levelPair in levels)
            {
                // Act
                WallLevel2D l2d = new WallLevel2D(levelPair.Key);

                // Assert
                Assert.True(l2d.AreLevel2DEqual(levelPair.Value));
            }
        }

        [Test]
        public void TestModifyWithActionDepth()
        {
            var level = new WallLevel2D(new[]
            {
                new[] { -1, 2, 2 }, // x: 0
                new[] { 1, 2, -1 }, // x: 1
                new[] { -1, -1, 0 } // x: 2
            });

            var level2 = new WallLevel2D(level,
                new PushPullAction(new Vector3(2, 2, 2), PushPullAction.Actions.PushForward));
            
            Assert.AreEqual(1, level2.Get(1, 1));
            Assert.AreEqual(2, level.Get(1, 1));
            
            var level3 = new WallLevel2D(level,
                new PushPullAction(new Vector3(0, 4, 0), PushPullAction.Actions.PullForward));
            
            Assert.AreEqual(3, level3.Get(0, 2));
            Assert.AreEqual(2, level2.Get(0, 2));
            Assert.AreEqual(2, level.Get(0, 2));
        }
    }
}