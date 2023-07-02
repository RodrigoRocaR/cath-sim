using System.Collections.Generic;
using System.Linq;
using Bots.DS.TreeModel;
using LevelDS;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.Bots.DS.TreeModel
{
    public class BlockFrontierTest
    {
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
                }
            };

            GameObject mockObject = new GameObject();
            var mockLevel = mockObject.AddComponent<Level>();

            foreach (var (levelValues, frontier) in cases)
            {
                mockLevel.NewMockLevel(levelValues);
                var f = new BlockFrontier(initialPostion);
                var obtainedFrontier = f.GetFrontier();
                Assert.AreEqual(frontier.Count, obtainedFrontier.Count);
                Assert.True(frontier.All(obtainedFrontier.Contains),
                    $"Obtained frontier does not match (order does not matter)\n" +
                    $"{string.Join("\n", frontier.Zip(obtainedFrontier, (elem1, elem2) => $"Expected: {elem1} --- Obtained: {elem2}").ToArray())}");
            }
        }
    }
}