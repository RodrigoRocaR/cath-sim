using System.Collections.Generic;
using Bots.Action;
using Bots.DS;
using NUnit.Framework;

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

            ActionStream actionStream = new ActionStream(new Level2D());
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
            Level2D level2D = new Level2D(new[]
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
            Level2D level2D = new Level2D(new[]
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
            Level2D level2D = new Level2D(new[]
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
    }
}