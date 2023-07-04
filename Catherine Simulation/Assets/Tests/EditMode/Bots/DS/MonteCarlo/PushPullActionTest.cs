using System.Collections.Generic;
using Bots.Algorithms;
using Bots.DS.MonteCarlo;
using LevelDS;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.Bots.DS.MonteCarlo
{
    public class PushPullActionTest
    {
        [Test]
        public void TestGetViableActions()
        {
            // Arrange
            GameObject mockObject = new GameObject();
            var mockLevel = mockObject.AddComponent<Level>();
            mockLevel.NewMockLevel(new[]
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
                new[] // x: 4
                {
                    new[] { -1, -1, -1 },
                    new[] { -1, 0, -1 },
                    new[] { -1, -1, -1 },
                },
            });

            BlockFrontier blockFrontier = new BlockFrontier(new HashSet<Vector3>
            {
                new Vector3(0, 1, 2),
                new Vector3(1, 1, 2),
                new Vector3(2, 1, 1),
                new Vector3(2, 2, 2),
                new Vector3(3, 1, 2),
                new Vector3(4, 1, 1)
            });

            Dictionary<Vector3, List<PushPullAction.Actions>> expected =
                new Dictionary<Vector3, List<PushPullAction.Actions>>
                {
                    {
                        new Vector3(0, 1, 2),
                        new List<PushPullAction.Actions> { PushPullAction.Actions.PushForward, PushPullAction.Actions.PullForward }
                    },
                    {
                        new Vector3(1, 1, 2),
                        new List<PushPullAction.Actions> { PushPullAction.Actions.PushForward, PushPullAction.Actions.PullForward }
                    },
                    {
                        new Vector3(2, 1, 1),
                        new List<PushPullAction.Actions>
                        {
                            PushPullAction.Actions.PushRight, PushPullAction.Actions.PullRight,
                            PushPullAction.Actions.PushLeft
                        }
                    },
                    {
                        new Vector3(2, 2, 2),
                        new List<PushPullAction.Actions> { PushPullAction.Actions.PushForward, PushPullAction.Actions.PullForward }
                    },
                    {
                        new Vector3(3, 1, 2),
                        new List<PushPullAction.Actions> { PushPullAction.Actions.PushForward, PushPullAction.Actions.PullForward }
                    },
                    {
                        new Vector3(4, 1, 1),
                        new List<PushPullAction.Actions> { PushPullAction.Actions.PushRight }
                    },
                };
            
            // Act
            var obtained = PushPullAction.GetViableActionsAsDict(blockFrontier);
            
            // Assert
            foreach (var (blockPos, viableActionsList) in obtained)
            {
                Assert.True(expected.ContainsKey(blockPos), "block pos not in expected");
                Assert.NotNull(obtained[blockPos]);
                CollectionAssert.AreEquivalent(expected, obtained);
            }
        }
    }
}