using System.Collections.Generic;
using System.Linq;
using Bots;
using Bots.DS;
using NUnit.Framework;

namespace Tests.EditMode.Bots
{
    public class BFSTest
    {
        [Test]
        public void TestReachesDesiredPosition()
        {
            // Arrange
            (int, int) startPos = (0, 0);
            Dictionary<Level2D, (int, int)> tests = new Dictionary<Level2D, (int, int)>
            {
                {
                    new Level2D(new[]
                    {
                        new[] { 0, 0, 0 },
                        new[] { 0, 0, 0 },
                        new[] { 0, 0, 0 },
                    }),
                    (0, 2)
                },
                {
                    new Level2D(new[]
                    {
                        new[] { 0, 1, 0, 1 },
                        new[] { 0, 1, 0, 1 },
                        new[] { 0, 1, 0, 1 },
                    }),
                    (0, 3)
                },
                {
                    new Level2D(new[]
                    {
                        new[] { 0, 2, 0, 0 },
                        new[] { 0, 2, 0, 1 },
                        new[] { 0, 1, 0, 2 },
                    }),
                    (1, 3)
                },
                {
                    new Level2D(new[]
                    {
                        new[] { 1, 2, -1, -1 },
                        new[] { 1, 2, 3, -1 },
                        new[] { 1, 2, 3, 4 },
                        new[] { 1, 2, 3, 5 },
                    }),
                    (2, 3)
                },
            };

            // Act
            foreach (var (level2D, desiredPos) in tests)
            {
                var bfs = new BFS(level2D);
                bfs.Explore(startPos.Item2, startPos.Item1);
                var lastPos = bfs.GetPath()[^1];
                // Assert
                Assert.AreEqual(desiredPos, lastPos);
            }
        }

        [Test]
        public void TestShortestPath()
        {
            // Arrange
            (int, int) startPos = (0, 0);
            Dictionary<Level2D, List<(int, int)>> tests = new Dictionary<Level2D, List<(int, int)>>
            {
                {
                    new Level2D(new[]
                    {
                        new[] { 0, 0, 0 },
                        new[] { 0, 0, 0 },
                        new[] { 0, 0, 0 },
                    }),
                    new List<(int, int)>
                    {
                        (0, 0),
                        (0, 1),
                        (0, 2)
                    }
                },
                {
                    new Level2D(new[]
                    {
                        new[] { 0, 0, 0, 2 },
                        new[] { 0, 2, 0, 2 },
                        new[] { 0, 2, 0, 1 },
                    }),
                    new List<(int, int)>
                    {
                        (0, 0),
                        (0, 1),
                        (0, 2),
                        (1, 2),
                        (2, 2),
                        (2, 3)
                    }
                },
                {
                    new Level2D(new[]
                    {
                        new[] { 1, 2, -1, -1 },
                        new[] { 1, 2, 3, -1 },
                        new[] { -1, 2, 3, 4 },
                        new[] { 1, 2, 3, 5 },
                    }),
                    new List<(int, int)>
                    {
                        (0, 0),
                        (1, 0),
                        (1, 1),
                        (2, 1),
                        (2, 2),
                        (2, 3)
                    }
                },
            };

            // Act
            foreach (var (level2D, desiredPos) in tests)
            {
                var bfs = new BFS(level2D);
                bfs.Explore(startPos.Item2, startPos.Item1);
                var lastPos = bfs.GetPath();
                // Assert
                Assert.AreEqual(desiredPos, lastPos);
            }
        }

        [Test]
        public void TestAddUnvisitedHeightDiff()
        {
            Level2D level2D = new Level2D(new[]
            {
                new[] { 1, 2, 0 },
                new[] { 2, 0, 0 },
                new[] { 2, 1, 0 },
            });
            var expectedElems = new[] { (2, 1), (1, 2) };

            BFS bfs = new BFS(level2D, true);
            bfs.SetPathToPos(new Dictionary<(int, int), ((int, int), int)> { { (1, 1), ((1, 1), 0) } });

            var method = MethodGetter.GetPrivateMethod(bfs, "EnqueueUnvisited");
            method.Invoke(bfs, new object[] { 1, 1 });

            var queue = bfs.GetUnvisited();
            Assert.AreEqual(expectedElems.Length, queue.Count);

            while (queue.Count > 0)
            {
                Assert.True(expectedElems.Contains(queue.Dequeue()));
            }
        }

        [Test]
        public void TestDoesNotRevisit()
        {
            Level2D level2D = new Level2D(new[]
            {
                new[] { 1, 0, 1 },
                new[] { 0, 0, 0 },
                new[] { 1, 0, 1 },
            });
            var expectedElems = new[] { (2, 1), (1, 2), (1, 0) };

            BFS bfs = new BFS(level2D, true);
            bfs.SetPathToPos(new Dictionary<(int, int), ((int, int), int)> { { (1, 1), ((1, 1), 0) } });
            bfs.SetVisited(new HashSet<(int, int)> { (0, 1) });

            var method = MethodGetter.GetPrivateMethod(bfs, "EnqueueUnvisited");
            method.Invoke(bfs, new object[] { 1, 1 });

            var queue = bfs.GetUnvisited();
            Assert.AreEqual(expectedElems.Length, queue.Count);
            while (queue.Count > 0) Assert.True(expectedElems.Contains(queue.Dequeue()));
        }
        
        [Test]
        public void TestDoesNotVisitEmptyBLocks()
        {
            Level2D level2D = new Level2D(new[]
            {
                new[] { -1, -1, -1 },
                new[] { -1, 0, -1 },
                new[] { -1, 0, 0 },
            });
            var expectedElems = new[] { (2, 1) };

            BFS bfs = new BFS(level2D, true);
            bfs.SetPathToPos(new Dictionary<(int, int), ((int, int), int)> { { (1, 1), ((1, 1), 0) } });

            var method = MethodGetter.GetPrivateMethod(bfs, "EnqueueUnvisited");
            method.Invoke(bfs, new object[] { 1, 1 });

            var queue = bfs.GetUnvisited();
            Assert.AreEqual(expectedElems.Length, queue.Count);
            while (queue.Count > 0) Assert.True(expectedElems.Contains(queue.Dequeue()));
        }
    }
}