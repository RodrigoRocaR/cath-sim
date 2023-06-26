using System.Collections.Generic;
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
                        new [] { 0, 0, 0 },
                        new [] { 0, 0, 0 },
                        new [] { 0, 0, 0 },
                    }),
                    (0, 2)
                },
            };
            
            // Act
            foreach (var (level2D, desiredPos) in tests)
            {
                var bfs = new BFS(level2D);
                bfs.Explore(startPos.Item2, startPos.Item1, () => { });
                var lastPos = bfs.GetPath()[^1];
                // Assert
                Assert.AreEqual(desiredPos, lastPos);
            }
        }
    }
}