using System.Collections.Generic;
using Bots.Action;
using Bots.DS;
using Bots.DS.MonteCarlo;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.Bots.Actions
{
    public class ActionStreamTest
    {
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
            
            Assert.AreEqual(examplePositions.Count-1, expectedMovements.Count, "The fixture is wrong");

            ActionStream actionStream = new ActionStream(new FloorLevel2D());
            actionStream.CreateFromPositions(examplePositions);
            
            var movements = actionStream.GetAsList();
            int n = movements.Count;
            
            Assert.AreEqual(examplePositions.Count-1, n);
            Assert.AreEqual(expectedMovements.Count, n);
            for (int i=0; i<n; i++)
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
                new [] { 0, 1},
                new [] { 1, 1},
            });

            ActionStream actionStream = new ActionStream(level2D);
            actionStream.CreateFromPositions(examplePositions);
            
            var movements = actionStream.GetAsList();
            int n = movements.Count;

            for (int i=0; i<n; i++)
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
                new [] { 0, 0, 0, 1, 0, 0, 3, 4, 5}
            });

            ActionStream actionStream = new ActionStream(level2D);
            actionStream.CreateFromPositions(examplePositions);
            
            var movements = actionStream.GetAsList();
            int n = movements.Count;

            for (int i=0; i<n; i++)
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
                new [] { 0, 1, 0, 0, 0, 4}
            });

            ActionStream actionStream = new ActionStream(level2D);
            actionStream.CreateFromPositions(examplePositions);
            
            var movements = actionStream.GetAsList();
            int n = movements.Count;

            for (int i=0; i<n; i++)
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
                new []{ 0, 0, 2 },
                new []{ 0, 0, 2 },
                new []{ 0, 2, 2 },
                new []{ 0, 0, 2 },
                new []{ 0, 2, 2 },
                new []{ 0, 0, 0 },
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
    }
}