using System.Collections.Generic;
using Bots.Algorithms;
using LevelDS;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.Bots.Algorithms
{
    public class WallHelperTest
    {
        [Test]
        public void TestGetWallHeight()
        {
            Dictionary<int[][][], int> testcases = new Dictionary<int[][][], int>
            {
                {
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { 0, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                            new[] { -1, -1, -1 },
                        },
                        new[] // x: 1
                        {
                            new[] { 0, 0, 0 },
                            new[] { 0, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, -1 },
                        },
                        new[] // x: 2
                        {
                            new[] { 0, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, -1 },
                        },
                    },
                    2
                },
                {
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { 0, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 1
                        {
                            new[] { 0, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 2
                        {
                            new[] { 0, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, -1 },
                        },
                    },
                    2
                },
                {
                    // detects ramp-like wall and selects lowest part
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { 0, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 1
                        {
                            new[] { 0, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, -1 },
                        },
                        new[] // x: 2
                        {
                            new[] { 0, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, -1 },
                            new[] { -1, -1, -1 },
                        },
                        new[] // x: 3
                        {
                            new[] { 0, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, -1 },
                            new[] { -1, -1, -1 },
                        },
                    },
                    2
                },
                {
                    // ignores holes with size 1
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { 0, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, -1 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 1
                        {
                            new[] { 0, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, -1 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, -1 },
                        },
                        new[] // x: 2
                        {
                            new[] { 0, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, -1 },
                            new[] { -1, -1, -1 },
                        },
                        new[] // x: 3
                        {
                            new[] { 0, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, -1 },
                            new[] { -1, -1, -1 },
                        },
                    },
                    2
                },
                {
                    // haves into account holes big enough
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { 0, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, -1 },
                        },
                        new[] // x: 1
                        {
                            new[] { 0, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, -1 },
                        },
                        new[] // x: 2
                        {
                            new[] { 0, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                        new[] // x: 3
                        {
                            new[] { 0, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, 0 },
                        },
                    },
                    3
                },
            };

            Vector3 initialPos = new Vector3(0, 1, 1);
            GameObject mockObject = new GameObject();
            var mockLevel = mockObject.AddComponent<Level>();

            foreach (var (levelValues, expectedHeight) in testcases)
            {
                mockLevel.NewMockLevel(levelValues);
                var wallhelper = new WallHelper(initialPos);
                Assert.AreEqual(expectedHeight, wallhelper.GetHeight());
            }
        }

        [Test]
        public void TestWallBoundaries()
        {
            Dictionary<int[][][], (int, int, int)> testcases = new Dictionary<int[][][], (int, int, int)>
            {
                {
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { 0, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                            new[] { -1, -1, -1 },
                        },
                        new[] // x: 1
                        {
                            new[] { 0, 0, 0 },
                            new[] { 0, -1, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, -1 },
                        },
                        new[] // x: 2
                        {
                            new[] { 0, 0, 0 },
                            new[] { -1, 0, 0 },
                            new[] { -1, -1, 0 },
                            new[] { -1, -1, -1 },
                        },
                    },
                    (0, 3, 1) // startX, stopX, startY
                },
                {
                    new[]
                    {
                        new[] // x: 0
                        {
                            new[] { -1, -1, -1, -1, -1, -1 },
                            new[] { -1, -1, -1, -1, -1, -1 },
                            new[] { -1, -1, -1, -1, -1, -1 },
                            new[] { -1, -1, -1, -1, -1, -1 },
                            new[] { -1, -1, -1, -1, -1, -1 },
                            new[] { -1, -1, -1, -1, -1, -1 },
                        },
                        new[] // x: 1
                        {
                            new[] { -1, -1, -1, -1, -1, -1 },
                            new[] { -1, -1, -1, -1, -1, -1 },
                            new[] { -1, -1, -1, -1, -1, -1 },
                            new[] { -1, -1, -1, -1, -1, -1 },
                            new[] { -1, -1, -1, -1, -1, -1 },
                            new[] { -1, -1, -1, -1, -1, -1 },
                        },
                        new[] // x: 2
                        {
                            new[] { -1, -1, -1, -1, -1, -1 },
                            new[] { -1, -1, -1, -1, -1, -1 },
                            new[] { -1, -1, -1, -1, 0, -1 }, 
                            new[] { -1, -1, -1, -1, -1, 0 }, // y:3
                            new[] { -1, -1, -1, -1, -1, 0 },
                            new[] { -1, -1, -1, -1, -1, -1 },
                        },
                        new[] // x: 3
                        {
                            new[] { -1, -1, -1, -1, -1, -1 },
                            new[] { -1, -1, -1, -1, -1, -1 },
                            new[] { -1, -1, -1, -1, 0, -1 },
                            new[] { -1, -1, -1, -1, -1, 0 },
                            new[] { -1, -1, -1, -1, -1, 0 },
                            new[] { -1, -1, -1, -1, -1, -1 },
                        },
                        new[] // x: 4
                        {
                            new[] { -1, -1, -1, -1, -1, -1 },
                            new[] { -1, -1, -1, -1, -1, -1 },
                            new[] { -1, -1, -1, -1, -1, -1 },
                            new[] { -1, -1, -1, -1, -1, -1 },
                            new[] { -1, -1, -1, -1, -1, -1 },
                            new[] { -1, -1, -1, -1, -1, -1 },
                        },
                    },
                    (2, 5, 3) // startX, stopX, startY
                },
            };

            List<Vector3> initialPos = new List<Vector3> { new Vector3(0, 1, 1), new Vector3(2, 3, 4) };
            GameObject mockObject = new GameObject();
            var mockLevel = mockObject.AddComponent<Level>();

            int i = 0;
            foreach (var (levelValues, (startX, stopX, startY)) in testcases)
            {
                mockLevel.NewMockLevel(levelValues);
                var wallhelper = new WallHelper(initialPos[i]);
                Assert.AreEqual(startX, wallhelper.GetStartX());
                Assert.AreEqual(stopX, wallhelper.GetStopX());
                Assert.AreEqual(startY, wallhelper.GetStartY());
                i++;
            }
        }
    }
}