using System.Collections.Generic;
using System.Linq;
using Bots.Algorithms;
using LevelDS;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.Bots.Algorithms
{
    public class BlockFrontierTest
    {
        private void TestGetFrontierWithCases(Dictionary<int[][][], List<Vector3>> cases, Vector3 initialPostion)
        {
            GameObject mockObject = new GameObject();
            var mockLevel = mockObject.AddComponent<Level>();

            foreach (var (levelValues, frontier) in cases)
            {
                mockLevel.NewMockLevel(levelValues);
                var f = new BlockFrontier(initialPostion, Level.GetGameMatrix());
                var obtainedFrontier = f.GetFrontier();
                Assert.AreEqual(frontier.Count, obtainedFrontier.Count);
                Assert.True(frontier.All(obtainedFrontier.Contains),
                    $"Obtained frontier does not match (order does not matter)\n" +
                    $"{string.Join("\n", frontier.Zip(obtainedFrontier, (elem1, elem2) => $"Expected: {elem1} --- Obtained: {elem2}").ToArray())}");
            }
        }

        [Test]
        public void TestGetFrontier()
        {
            Vector3 initialPostion = new Vector3(1, 0, 1);
            Dictionary<int[][][], List<Vector3>> cases = new Dictionary<int[][][], List<Vector3>>
            {
                {
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x: 1
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 2
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 3
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                    },
                    new List<Vector3>
                    {
                        new Vector3(0, 1, 2),
                        new Vector3(1, 1, 2),
                        new Vector3(2, 1, 1),
                        new Vector3(2, 2, 2),
                        new Vector3(3, 1, 2),
                    }
                },
                {
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x: 1
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 2
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 3
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                        },
                    },
                    new List<Vector3>
                    {
                        new Vector3(0, 1, 2),
                        new Vector3(1, 1, 2),
                        new Vector3(2, 1, 1),
                        new Vector3(2, 2, 2),
                        new Vector3(3, 2, 1),
                    }
                },
                {
                    // trapped in walls
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, 0, 0 }, // y:1
                            new[] { -1, 0, 0 }, // y:2
                            new[] { -1, 0, 0 }, // y:3
                        },
                        new[] // x: 1
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 2
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 3
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                    },
                    new List<Vector3>
                    {
                        new Vector3(0, 1, 1),
                        new Vector3(1, 1, 2),
                        new Vector3(2, 1, 1),
                        new Vector3(3, 1, 2),
                    }
                }
            };

            TestGetFrontierWithCases(cases, initialPostion);
        }

        [Test]
        public void TestWithAsymmetricPlayerPos()
        {
            Vector3 initialPostion = new Vector3(2, 6, 3);

            GameObject mockObject = new GameObject();
            var mockLevel = mockObject.AddComponent<Level>();
            mockLevel.NewMockLevel(new[]
            {
                new[] // x: 0
                {
                    new[] { -1, -1, -1, -1, -1 }, // y:0
                    new[] { -1, -1, -1, -1, -1 }, // y:1
                    new[] { -1, -1, -1, -1, -1 }, // y:2
                    new[] { -1, -1, -1, -1, -1 }, // y:3
                    new[] { -1, -1, -1, -1, -1 }, // y:4
                    new[] { -1, -1, -1, -1, -1 }, // y:5
                    new[] { -1, -1, -1, 0, -1 }, // y:6
                    new[] { -1, -1, -1, -1, 0 }, // y:7
                },
                new[] // x: 1
                {
                    new[] { -1, -1, -1, -1, -1 }, // y:0
                    new[] { -1, -1, -1, -1, -1 }, // y:1
                    new[] { -1, -1, -1, -1, -1 }, // y:2
                    new[] { -1, -1, -1, -1, -1 }, // y:3
                    new[] { -1, -1, -1, -1, -1 }, // y:4
                    new[] { -1, -1, -1, -1, -1 }, // y:5
                    new[] { -1, -1, -1, 0, -1 }, // y:6
                    new[] { -1, -1, 0, -1, 0 }, // y:7
                },
                new[] // x: 2
                {
                    new[] { -1, -1, -1, -1, -1 }, // y:0
                    new[] { -1, -1, -1, -1, -1 }, // y:1
                    new[] { -1, -1, -1, -1, -1 }, // y:2
                    new[] { -1, -1, -1, -1, -1 }, // y:3
                    new[] { -1, -1, -1, -1, -1 }, // y:4
                    new[] { -1, -1, -1, -1, -1 }, // y:5
                    new[] { -1, -1, -1, 0, -1 }, // y:6
                    new[] { -1, -1, -1, -1, 0 }, // y:7
                },
            });

            var expected = new List<Vector3>
            {
                new Vector3(2, 7, 4),
                new Vector3(1, 7, 2),
                new Vector3(1, 7, 4),
                new Vector3(0, 7, 4),
            };

            var obtained = new BlockFrontier(initialPostion, Level.GetGameMatrix()).GetFrontier();

            CollectionAssert.AreEquivalent(expected, obtained);
        }

        [Test]
        public void TestWithWall()
        {
            Vector3 initialPostion = new Vector3(0, 0, 1);
            Dictionary<int[][][], List<Vector3>> cases = new Dictionary<int[][][], List<Vector3>>
            {
                {
                    // test cannot get back up
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x: 1
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                        },
                        new[] // x: 2
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, 0, 0 },
                        },
                        new[] // x: 3
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 4
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                    },
                    new List<Vector3>
                    {
                        new Vector3(0, 1, 2),
                        new Vector3(1, 1, 1),
                    }
                },
                {
                    // hanging from first block and wall at x = 2
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x: 1
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 2
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                        },
                        new[] // x: 3
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 4
                        {
                            new[] { -1, -1, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                    },
                    new List<Vector3>
                    {
                        new Vector3(0, 1, 2),
                        new Vector3(1, 1, 2),
                        new Vector3(2, 1, 1),
                        new Vector3(3, 1, 2),
                        new Vector3(4, 1, 1),
                        new Vector3(4, 2, 2),
                    }
                },
                {
                    // test does not get up when there is no space
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x: 1
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                        },
                        new[] // x: 2
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, 0, 0 },
                        },
                        new[] // x: 3
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 4
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                    },
                    new List<Vector3>
                    {
                        new Vector3(0, 1, 2),
                        new Vector3(1, 1, 1),
                        new Vector3(3, 1, 1),
                        new Vector3(4, 1, 2),
                        new Vector3(3, 2, 2),
                        new Vector3(2, 2, 1),
                    }
                },
            };
            TestGetFrontierWithCases(cases, initialPostion);
        }

        [Test]
        public void TestWhileHanging()
        {
            Vector3 initialPostion = new Vector3(0, 0, 0);
            Dictionary<int[][][], List<Vector3>> cases = new Dictionary<int[][][], List<Vector3>>
            {
                {
                    // hanging from first block and wall at x = 2
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x: 1
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 2
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                        },
                        new[] // x: 3
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 4
                        {
                            new[] { -1, -1, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                    },
                    new List<Vector3>
                    {
                        new Vector3(0, 1, 2),
                        new Vector3(1, 1, 2),
                        new Vector3(2, 1, 1),
                        new Vector3(3, 1, 2),
                        new Vector3(4, 1, 1),
                        new Vector3(4, 2, 2),
                    }
                },
            };
            TestGetFrontierWithCases(cases, initialPostion);
        }

        [Test]
        public void TestHangingCannotGetUp()
        {
            Vector3 initialPostion = new Vector3(3, 0, 0);
            Dictionary<int[][][], List<Vector3>> cases = new Dictionary<int[][][], List<Vector3>>
            {
                {
                    // has space on both sides
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x: 1
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 2
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 3
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, 0, 0 },
                        },
                        new[] // x: 4
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 5
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 6
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                    },
                    new List<Vector3>
                    {
                        new Vector3(0, 1, 2),
                        new Vector3(1, 1, 1),
                        new Vector3(5, 1, 1),
                        new Vector3(6, 1, 2),
                        // above blocks
                        new Vector3(5, 2, 2),
                        new Vector3(4, 2, 2),
                        new Vector3(3, 2, 1),
                        new Vector3(2, 2, 2),
                        new Vector3(1, 2, 2),
                    }
                },
                {
                    // runs out of blocks on the right, but has space on the left
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x: 1
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 2
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 3
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, 0, 0 },
                        },
                        new[] // x: 4
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 5
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 6
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, 0, 0 },
                        },
                    },
                    new List<Vector3>
                    {
                        new Vector3(0, 1, 2),
                        new Vector3(1, 1, 1),
                    }
                },
                {
                    // only has space on the right
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, 0, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x: 1
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 2
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 3
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, 0, 0 },
                        },
                        new[] // x: 4
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 5
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                        },
                        new[] // x: 6
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                    },
                    new List<Vector3>
                    {
                        new Vector3(5, 1, 1),
                        new Vector3(6, 1, 2),
                    }
                },
                {
                    // has no space
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, 0, 0 }, // y:2
                        },
                        new[] // x: 1
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 2
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 3
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, 0, 0 },
                        },
                        new[] // x: 4
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 5
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 6
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, 0, 0 },
                        },
                    },
                    new List<Vector3>
                    {
                    }
                },
            };
            TestGetFrontierWithCases(cases, initialPostion);
        }

        [Test]
        public void TestNeedsToHangButDoesNotStartHanging()
        {
            Vector3 initialPostion = new Vector3(3, 0, 1);
            Dictionary<int[][][], List<Vector3>> cases = new Dictionary<int[][][], List<Vector3>>
            {
                {
                    // has space on both sides
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x: 1
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                        },
                        new[] // x: 2
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 3
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 4
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 5
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                        },
                        new[] // x: 6
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                    },
                    new List<Vector3>
                    {
                        new Vector3(3, 1, 2),
                        new Vector3(4, 1, 2),
                        new Vector3(5, 1, 1),
                        new Vector3(6, 1, 2),
                        new Vector3(2, 1, 2),
                        new Vector3(1, 1, 1),
                        new Vector3(0, 1, 2),
                    }
                },
            };
            TestGetFrontierWithCases(cases, initialPostion);
        }

        [Test]
        public void TestCanHangOnZAxis()
        {
            Vector3 initialPostion = new Vector3(0, 1, 1);
            Dictionary<int[][][], List<Vector3>> cases = new Dictionary<int[][][], List<Vector3>>
            {
                {
                    // basic: hang backwards one time
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { -1, -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0, 0 }, // y:1
                            new[] { -1, -1, 0, 0 }, // y:2
                        },
                        new[] // x: 1
                        {
                            new[] { -1, -1, 0, 0 },
                            new[] { -1, -1, -1, 0 },
                            new[] { -1, -1, -1, 0 },
                        },
                        new[] // x: 2
                        {
                            new[] { -1, -1, 0, 0 },
                            new[] { -1, -1, -1, 0 },
                            new[] { -1, -1, -1, 0 },
                        },
                        new[] // x: 3
                        {
                            new[] { -1, -1, 0, 0 },
                            new[] { -1, -1, 0, 0 },
                            new[] { -1, -1, -1, 0 },
                        },
                        new[] // x: 4
                        {
                            new[] { -1, -1, 0, 0 },
                            new[] { -1, -1, 0, 0 },
                            new[] { -1, -1, -1, 0 },
                        },
                    },
                    new List<Vector3>
                    {
                        new Vector3(3, 2, 3),
                        new Vector3(4, 2, 3),
                        // blocks that can trigger after hanging:
                        new Vector3(2, 1, 3),
                        new Vector3(1, 1, 3),
                        new Vector3(0, 1, 2),
                        new Vector3(3, 1, 2),
                    }
                },
                {
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { -1, -1, 0, 0, 0 },
                            new[] { -1, -1, 0, 0, 0 },
                            new[] { -1, -1, 0, 0, 0 },
                        },
                        new[] // x: 1
                        {
                            new[] { -1, -1, 0, 0, 0 },
                            new[] { -1, -1, -1, -1, 0 },
                            new[] { -1, -1, -1, -1, 0 },
                        },
                        new[] // x: 2
                        {
                            new[] { -1, -1, 0, 0, 0 },
                            new[] { -1, -1, -1, -1, 0 },
                            new[] { -1, -1, -1, -1, 0 },
                        },
                        new[] // x: 3
                        {
                            new[] { -1, -1, 0, 0, 0 },
                            new[] { -1, -1, -1, -1, 0 },
                            new[] { -1, -1, -1, -1, -1 },
                        },
                    },
                    new List<Vector3>
                    {
                        new Vector3(2, 2, 4),
                    }
                },
            };
            TestGetFrontierWithCases(cases, initialPostion);
        }

        [Test]
        public void TestMultiLevelFrontier()
        {
            Vector3 initialPostion = new Vector3(4, 0, 1);
            Dictionary<int[][][], List<Vector3>> cases = new Dictionary<int[][][], List<Vector3>>
            {
                {
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { -1, 0, -1, -1 }, // y:0
                            new[] { -1, -1, 0, -1 }, // y:1
                            new[] { -1, -1, 0, 0 }, // y:2
                            new[] { -1, -1, -1, 0 }, // y:3
                            new[] { -1, -1, -1, 0 }, // y:4
                        },
                        new[] // x: 1
                        {
                            new[] { -1, 0, -1, -1 }, // y:0
                            new[] { -1, 0, 0, -1 }, // y:1
                            new[] { -1, -1, 0, 0 }, // y:2
                            new[] { -1, -1, -1, 0 }, // y:3
                            new[] { -1, -1, -1, 0 }, // y:4
                        },
                        new[] // x: 2
                        {
                            new[] { -1, 0, -1, -1 }, // y:0
                            new[] { -1, -1, 0, -1 }, // y:1
                            new[] { -1, -1, 0, 0 }, // y:2
                            new[] { -1, -1, -1, 0 }, // y:3
                            new[] { -1, -1, -1, 0 }, // y:4
                        },
                        new[] // x: 3
                        {
                            new[] { -1, 0, -1, -1 }, // y:0
                            new[] { -1, -1, 0, -1 }, // y:1
                            new[] { -1, -1, 0, 0 }, // y:2
                            new[] { -1, -1, -1, 0 }, // y:3
                            new[] { -1, -1, -1, 0 }, // y:4
                        },
                        new[] // x: 4
                        {
                            new[] { -1, 0, -1, -1 }, // y:0
                            new[] { -1, -1, 0, -1 }, // y:1
                            new[] { -1, -1, 0, 0 }, // y:2
                            new[] { -1, -1, -1, 0 }, // y:3
                            new[] { -1, -1, -1, 0 }, // y:4
                        },
                        new[] // x: 5
                        {
                            new[] { -1, 0, -1, -1 }, // y:0
                            new[] { -1, -1, 0, -1 }, // y:1
                            new[] { -1, -1, 0, 0 }, // y:2
                            new[] { -1, -1, -1, 0 }, // y:3
                            new[] { -1, -1, -1, 0 }, // y:4
                        },
                    },
                    new List<Vector3>
                    {
                        new Vector3(3, 1, 2),
                        new Vector3(4, 1, 2),
                        new Vector3(5, 1, 2),
                        new Vector3(2, 1, 2),
                        new Vector3(1, 1, 1),
                        new Vector3(1, 2, 2),
                        new Vector3(0, 1, 2),
                        // Row above
                        new Vector3(0, 3, 3),
                        new Vector3(1, 3, 3),
                        new Vector3(2, 3, 3),
                        new Vector3(3, 3, 3),
                        new Vector3(4, 3, 3),
                        new Vector3(5, 3, 3),
                    }
                },
                {
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { -1, 0, -1, -1, -1 }, // y:0
                            new[] { -1, -1, 0, -1, -1 }, // y:1
                            new[] { -1, -1, 0, -1, -1 }, // y:2
                            new[] { -1, -1, 0, -1, -1 }, // y:3
                            new[] { -1, -1, 0, 0, 0 }, // y:4
                            new[] { -1, -1, -1, -1, 0 }, // y:5
                            new[] { -1, -1, -1, -1, 0 }, // y:6
                            new[] { -1, -1, -1, -1, -1 }, // y:7
                        },
                        new[] // x: 1
                        {
                            new[] { -1, 0, -1, -1, -1 }, // y:0
                            new[] { -1, 0, 0, -1, -1 }, // y:1
                            new[] { -1, -1, 0, -1, -1 }, // y:2
                            new[] { -1, -1, 0, -1, -1 }, // y:3
                            new[] { -1, -1, 0, 0, 0 }, // y:4
                            new[] { -1, -1, -1, -1, 0 }, // y:5
                            new[] { -1, -1, -1, -1, 0 }, // y:6
                            new[] { -1, -1, -1, -1, -1 }, // y:7
                        },
                        new[] // x: 2
                        {
                            new[] { -1, 0, -1, -1, -1 }, // y:0
                            new[] { -1, 0, 0, -1, -1 }, // y:1
                            new[] { -1, 0, 0, -1, -1 }, // y:2
                            new[] { -1, -1, 0, -1, -1 }, // y:3
                            new[] { -1, -1, 0, 0, 0 }, // y:4
                            new[] { -1, -1, -1, -1, 0 }, // y:5
                            new[] { -1, -1, -1, -1, 0 }, // y:6
                            new[] { -1, -1, -1, -1, -1 }, // y:7
                        },
                        new[] // x: 3
                        {
                            new[] { -1, 0, -1, -1, -1 }, // y:0
                            new[] { -1, 0, 0, -1, -1 }, // y:1
                            new[] { -1, 0, 0, -1, -1 }, // y:2
                            new[] { -1, 0, 0, -1, -1 }, // y:3
                            new[] { -1, -1, 0, 0, 0 }, // y:4
                            new[] { -1, -1, -1, -1, 0 }, // y:5
                            new[] { -1, -1, -1, -1, 0 }, // y:6
                            new[] { -1, -1, -1, -1, -1 }, // y:7
                        },
                        new[] // x: 4
                        {
                            new[] { -1, 0, -1, -1, -1 }, // y:0
                            new[] { -1, -1, 0, -1, -1 }, // y:1
                            new[] { -1, -1, 0, -1, -1 }, // y:2
                            new[] { -1, -1, 0, -1, -1 }, // y:3
                            new[] { -1, -1, 0, 0, 0 }, // y:4
                            new[] { -1, -1, -1, -1, 0 }, // y:5
                            new[] { -1, -1, -1, -1, 0 }, // y:6
                            new[] { -1, -1, -1, -1, -1 }, // y:7
                        },
                        new[] // x: 5
                        {
                            new[] { -1, 0, -1, -1, -1 }, // y:0
                            new[] { -1, -1, 0, -1, -1 }, // y:1
                            new[] { -1, -1, 0, -1, -1 }, // y:2
                            new[] { -1, -1, 0, -1, -1 }, // y:3
                            new[] { -1, -1, 0, 0, 0 }, // y:4
                            new[] { -1, -1, -1, -1, 0 }, // y:5
                            new[] { -1, -1, -1, -1, 0 }, // y:6
                            new[] { -1, -1, -1, -1, -1 }, // y:7
                        },
                    },
                    new List<Vector3>
                    {
                        new Vector3(3, 1, 1),
                        new Vector3(4, 1, 2),
                        new Vector3(5, 1, 2),
                        new Vector3(0, 1, 2),
                        // stair
                        new Vector3(1, 1, 1),
                        new Vector3(1, 2, 2),
                        new Vector3(2, 2, 1),
                        new Vector3(2, 3, 2),
                        new Vector3(3, 3, 1),
                        new Vector3(3, 4, 2),
                        // next level
                        new Vector3(0, 5, 4),
                        new Vector3(1, 5, 4),
                        new Vector3(2, 5, 4),
                        new Vector3(3, 5, 4),
                        new Vector3(4, 5, 4),
                        new Vector3(5, 5, 4),
                    }
                },
            };
            TestGetFrontierWithCases(cases, initialPostion);
        }

        [Test]
        public void TestCanHangOnExtremes()
        {
            Vector3 initialPostion = new Vector3(4, 1, 1);
            Dictionary<int[][][], List<Vector3>> cases = new Dictionary<int[][][], List<Vector3>>
            {
                {
                    // Hangs on the right extreme and cannot get up
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x: 1
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 2
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 3
                        {
                            new[] { -1, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                        },
                    },
                    new List<Vector3>
                    {
                        new Vector3(0, 1, 2),
                        new Vector3(1, 1, 1),
                        new Vector3(1, 2, 2),
                        new Vector3(2, 1, 2),
                        new Vector3(3, 1, 1),
                        new Vector3(3, 2, 2),
                    }
                },
            };
            TestGetFrontierWithCases(cases, initialPostion);
        }
    }
}