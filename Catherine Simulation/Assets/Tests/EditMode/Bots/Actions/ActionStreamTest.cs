using System.Collections.Generic;
using Bots.Action;
using Bots.DS;
using Bots.DS.MonteCarlo;
using LevelDS;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.Bots.Actions
{
    public class ActionStreamTest
    {
        private struct TestcaseForPushPullActions
        {
            public Vector3 InitialPos;
            public Level2D Level2D;
            public int[][][] LevelValues;
            public List<PushPullAction> PushPullActions;
            public List<Action> ExpectedMovements;
        }

        private void TestTestcaseForPushPullActions(TestcaseForPushPullActions tc)
        {
            GameObject mockObject = new GameObject();
            var mockLevel = mockObject.AddComponent<Level>();
            mockLevel.NewMockLevel(tc.LevelValues);

            ActionStream actionStream = new ActionStream(tc.Level2D);
            actionStream.CreateFromPushPullActions(tc.InitialPos, tc.PushPullActions);
            CollectionAssert.AreEqual(tc.ExpectedMovements, actionStream.GetAsList());
        }


        [Test]
        public void TestCreateFromPositions()
        {
            List<(int, int)> examplePositions = new List<(int, int)>
            {
                (0, 0), // x, z
                (0, 1),
                (1, 1),
                (1, 0),
                (0, 0)
            };

            List<Action> expectedMovements = new List<Action>
            {
                Action.Forward,
                Action.Right,
                Action.Backward,
                Action.Left,
            };

            Assert.AreEqual(examplePositions.Count - 1, expectedMovements.Count, "The fixture is wrong");

            ActionStream actionStream = new ActionStream(new FloorLevel2D());
            actionStream.CreateFromPositions(examplePositions);

            var movements = actionStream.GetAsList();
            int n = movements.Count;

            Assert.AreEqual(examplePositions.Count - 1, n);
            Assert.AreEqual(expectedMovements.Count, n);
            for (int i = 0; i < n; i++)
            {
                Assert.AreEqual(expectedMovements[i], movements[i]);
            }
        }

        [Test]
        public void TranslateActionsTo3D()
        {
            List<(int, int)> examplePositions = new List<(int, int)>
            {
                (0, 0), // x, z
                (1, 0),
                (1, 1),
                (0, 1),
                (0, 0)
            };

            List<Action> expectedMovements = new List<Action>
            {
                Action.Right,
                Action.Jump,
                Action.Forward,
                Action.Left,
                Action.Backward,
                Action.Jump
            };
            Level2D level2D = new FloorLevel2D(new[]
            {
                new[] { 0, 1 },
                new[] { 1, 1 },
            });

            ActionStream actionStream = new ActionStream(level2D);
            actionStream.CreateFromPositions(examplePositions);

            var movements = actionStream.GetAsList();
            int n = movements.Count;

            for (int i = 0; i < n; i++)
            {
                Assert.AreEqual(expectedMovements[i], movements[i]);
            }
        }

        [Test]
        public void TestDoesNotAddExtraJumps()
        {
            List<(int, int)> examplePositions = new List<(int, int)>
            {
                (0, 0), // x, z
                (0, 1),
                (0, 2),
                (0, 3),
                (0, 4),
                (0, 5),
            };

            List<Action> expectedMovements = new List<Action>
            {
                Action.Forward,
                Action.Forward,
                Action.Forward,
                Action.Jump,
                Action.Forward,
                Action.Jump,
                Action.Forward
            };
            Level2D level2D = new FloorLevel2D(new[]
            {
                new[] { 0, 0, 0, 1, 0, 0, 3, 4, 5 }
            });

            ActionStream actionStream = new ActionStream(level2D);
            actionStream.CreateFromPositions(examplePositions);

            var movements = actionStream.GetAsList();
            int n = movements.Count;

            for (int i = 0; i < n; i++)
            {
                Assert.AreEqual(expectedMovements[i], movements[i]);
            }
        }

        [Test]
        public void TestJumpsDown()
        {
            List<(int, int)> examplePositions = new List<(int, int)>
            {
                (0, 0), // x, z
                (0, 1),
                (0, 2),
                (0, 3)
            };

            List<Action> expectedMovements = new List<Action>
            {
                Action.Forward,
                Action.Jump,
                Action.Forward,
                Action.Jump,
                Action.Forward
            };
            Level2D level2D = new FloorLevel2D(new[]
            {
                new[] { 0, 1, 0, 0, 0, 4 }
            });

            ActionStream actionStream = new ActionStream(level2D);
            actionStream.CreateFromPositions(examplePositions);

            var movements = actionStream.GetAsList();
            int n = movements.Count;

            for (int i = 0; i < n; i++)
            {
                Assert.AreEqual(expectedMovements[i], movements[i]);
            }
        }

        [Test]
        public void TestCreateFromPushPullActions()
        {
            Vector3 initialPos = new Vector3(0, 0);

            Level2D level2D = new FloorLevel2D(new[]
            {
                new[] { 0, 0, 2 },
                new[] { 0, 0, 2 },
                new[] { 0, 2, 2 },
                new[] { 0, 0, 2 },
                new[] { 0, 2, 2 },
                new[] { 0, 0, 0 },
            });

            List<PushPullAction> pushPullActions = new List<PushPullAction>
            {
                new PushPullAction(new Vector3(3, 0, 2), PushPullAction.Actions.PushForward)
            };

            List<Action> expectedMovements = new List<Action>
            {
                Action.Right,
                Action.Right,
                Action.Right,
                Action.Forward,
                Action.Forward, // look at the block
                Action.Push
            };

            ActionStream actionStream = new ActionStream(level2D);
            actionStream.CreateFromPushPullActions(initialPos, pushPullActions);

            CollectionAssert.AreEqual(expectedMovements, actionStream.GetAsList());
        }

        [Test]
        public void TestCreateFromPushPullActionsHanging()
        {
            List<TestcaseForPushPullActions> tests = new List<TestcaseForPushPullActions>
            {
                new TestcaseForPushPullActions // Basic hanging straight
                {
                    InitialPos = new Vector3(0, 0, 1),
                    Level2D = new FloorLevel2D(new[]
                    {
                        new[] { -1, 0, 2 },
                        new[] { -1, 0, 2 },
                        new[] { -1, 2, 2 },
                        new[] { -1, 0, 2 },
                        new[] { -1, 2, 2 },
                        new[] { -1, 0, 2 },
                    }),
                    LevelValues = new[]
                    {
                        new[] // x:0
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x:1
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x:2
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, 0, 0 }, // y:1
                            new[] { -1, 0, 0 }, // y:2
                        },
                        new[] // x:3
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x:4
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, 0, 0 }, // y:1
                            new[] { -1, 0, 0 }, // y:2
                        },
                        new[] // x:5
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                    },
                    PushPullActions = new List<PushPullAction>
                    {
                        new PushPullAction(new Vector3(5, 1, 2), PushPullAction.Actions.PushForward)
                    },
                    ExpectedMovements = new List<Action>
                    {
                        Action.Right, // x:1
                        Action.Backward, // start hanging
                        Action.Right, // x:2
                        Action.Right, // x:3
                        Action.Right, // x:4
                        Action.Right, // x:5
                        Action.Forward, // get up from hanging
                        Action.Forward, // look at the block
                        Action.Push
                    }
                },
                new TestcaseForPushPullActions // Does not hang unnecessarily
                {
                    InitialPos = new Vector3(0, 0, 1),
                    Level2D = new FloorLevel2D(new[]
                    {
                        new[] { -1, 0, 2 },
                        new[] { -1, 0, 2 },
                        new[] { -1, 1, 2 },
                        new[] { -1, 0, 2 },
                        new[] { -1, 1, 2 },
                        new[] { -1, 0, 2 },
                    }),
                    LevelValues = new[]
                    {
                        new[] // x:0
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x:1
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x:2
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, 0, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x:3
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x:4
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, 0, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x:5
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                    },
                    PushPullActions = new List<PushPullAction>
                    {
                        new PushPullAction(new Vector3(5, 1, 2), PushPullAction.Actions.PushForward)
                    },
                    ExpectedMovements = new List<Action>
                    {
                        Action.Right, // x:1
                        Action.Right, // look at block
                        Action.Jump, // x:2
                        Action.Right, // look at next
                        Action.Jump, // x:3
                        Action.Right, // look
                        Action.Jump, // x:4
                        Action.Right, // x:look
                        Action.Jump, // x:5
                        Action.Forward, // look at block
                        Action.Push
                    }
                },
                new TestcaseForPushPullActions // Hangs when there is no foundation and forward / backward
                {
                    InitialPos = new Vector3(0, 0, 1),
                    Level2D = new FloorLevel2D(new[]
                    {
                        new[] { -1, 0, 2 },
                        new[] { -1, 0, 2 },
                        new[] { -1, -1, 2 },
                        new[] { -1, -1, 2 },
                        new[] { -1, -1, 2 },
                        new[] { -1, 0, 2 },
                    }),
                    LevelValues = new[]
                    {
                        new[] // x:0
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x:1
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x:2
                        {
                            new[] { -1, -1, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x:3
                        {
                            new[] { -1, -1, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x:4
                        {
                            new[] { -1, -1, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                        new[] // x:5
                        {
                            new[] { -1, 0, 0 }, // y:0
                            new[] { -1, -1, 0 }, // y:1
                            new[] { -1, -1, 0 }, // y:2
                        },
                    },
                    PushPullActions = new List<PushPullAction>
                    {
                        new PushPullAction(new Vector3(5, 1, 2), PushPullAction.Actions.PushForward)
                    },
                    ExpectedMovements = new List<Action>
                    {
                        Action.Right, // x:1
                        Action.Backward, // start hanging
                        Action.Right, // go forward hanging
                        Action.Right, // x:2
                        Action.Right, // x:3
                        Action.Right, // x:4
                        Action.Right, // go backward hanging
                        Action.Right, // x:5
                        Action.Forward, // get up from hanging
                        Action.Forward, // look at the block
                        Action.Push
                    }
                },
            };

            foreach (var test in tests) TestTestcaseForPushPullActions(test);
        }
    }
}