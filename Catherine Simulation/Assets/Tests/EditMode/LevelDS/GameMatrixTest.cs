using LevelDS;
using NUnit.Framework;

namespace Tests.EditMode.LevelDS
{
    public class GameMatrixTest
    {
        [Test]
        public void IncreaseSizexyz()
        {
            // Arrange
            var gameMatrix = new GameMatrix(3, 3, 3);
            
            // Act
            gameMatrix.SetBlockInt(4, 4, 4, 1);
            
            // Assert
            Assert.AreEqual(gameMatrix.Width, 5);
            Assert.AreEqual(gameMatrix.Height, 5);
            Assert.AreEqual(gameMatrix.Depth, 5);
        }
    }
}