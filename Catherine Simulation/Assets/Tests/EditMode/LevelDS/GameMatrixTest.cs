using LevelDS;
using NUnit.Framework;

namespace Tests.EditMode.LevelDS
{
    public class GameMatrixTest
    {
        [Test]
        public void IncreaseSizeRightxyz()
        {
            // Arrange
            var gameMatrix = new GameMatrix(3, 3, 3);
            
            // Act
            gameMatrix.SetBlockInt(4, 4, 4, 1);
            
            // Assert
            Assert.AreEqual(gameMatrix.Width, 5);
            Assert.AreEqual(gameMatrix.Height, 5);
            Assert.AreEqual(gameMatrix.Depth, 5);
            
            for (int i=0; i<gameMatrix.Width; i++)
            {
                for (int j=0; j<gameMatrix.Height; j++)
                {
                    for (int k=0; k<gameMatrix.Depth; k++)
                    {
                        if (i == 4 && j == 4 && k == 4)
                        {
                            Assert.AreEqual(gameMatrix.GetBlockInt(i, j, k), 1);
                        }
                        else
                        {
                            Assert.AreEqual(gameMatrix.GetBlockInt(i, j, k), -1);
                        }
                        Assert.AreEqual(gameMatrix.GetBlock(i, j, k), null);
                    }
                }
            }
        }

        [Test]
        public void IncreaseSizeRightIrregularlyxyz()
        {
            // Arrange
            var gameMatrix = new GameMatrix(3, 3, 3);
            
            // Act
            gameMatrix.SetBlockInt(4, 10, 5, 1);
            
            // Assert
            Assert.AreEqual(gameMatrix.Width, 5);
            Assert.AreEqual(gameMatrix.Height, 11);
            Assert.AreEqual(gameMatrix.Depth, 6);
            
            for (int i=0; i<gameMatrix.Width; i++)
            {
                for (int j=0; j<gameMatrix.Height; j++)
                {
                    for (int k=0; k<gameMatrix.Depth; k++)
                    {
                        if (i == 4 && j == 10 && k == 5)
                        {
                            Assert.AreEqual(gameMatrix.GetBlockInt(i, j, k), 1);
                        }
                        else
                        {
                            Assert.AreEqual(gameMatrix.GetBlockInt(i, j, k), -1);
                        }
                        Assert.AreEqual(gameMatrix.GetBlock(i, j, k), null);
                    }
                }
            }
        }
        
        [Test]
        public void IncreaseSizeLeftxyz()
        {
            // Arrange
            var gameMatrix = new GameMatrix(3, 3, 3);
            
            // Act
            gameMatrix.SetBlockInt(-1, -1, -1, 1);
            
            // Assert
            Assert.AreEqual(gameMatrix.Width, 4);
            Assert.AreEqual(gameMatrix.Height, 4);
            Assert.AreEqual(gameMatrix.Depth, 4);
            
            for (int i=0; i<gameMatrix.Width; i++)
            {
                for (int j=0; j<gameMatrix.Height; j++)
                {
                    for (int k=0; k<gameMatrix.Depth; k++)
                    {
                        if (i == -1 && j == -1 && k == -1)
                        {
                            Assert.AreEqual(gameMatrix.GetBlockInt(i, j, k), 1);
                        }
                        else
                        {
                            Assert.AreEqual(gameMatrix.GetBlockInt(i, j, k), -1);
                        }
                        Assert.AreEqual(gameMatrix.GetBlock(i, j, k), null);
                    }
                }
            }
        }
        
        [Test]
        public void IncreaseSizeLeftIrregularxyz()
        {
            // Arrange
            var gameMatrix = new GameMatrix(3, 3, 3);
            
            // Act
            gameMatrix.SetBlockInt(-1, 2, -2, 1);
            
            // Assert
            Assert.AreEqual(gameMatrix.Width, 4);
            Assert.AreEqual(gameMatrix.Height, 3);
            Assert.AreEqual(gameMatrix.Depth, 5);
            
            for (int i=0; i<gameMatrix.Width; i++)
            {
                for (int j=0; j<gameMatrix.Height; j++)
                {
                    for (int k=0; k<gameMatrix.Depth; k++)
                    {
                        if (i == -1 && j == 2 && k == -2)
                        {
                            Assert.AreEqual(gameMatrix.GetBlockInt(i, j, k), 1);
                        }
                        else
                        {
                            Assert.AreEqual(gameMatrix.GetBlockInt(i, j, k), -1);
                        }
                        Assert.AreEqual(gameMatrix.GetBlock(i, j, k), null);
                    }
                }
            }
        }
        
        [Test]
        public void DoesNotIncreaseSizeWhenInRange()
        {
            // Arrange
            var gameMatrix = new GameMatrix(3, 3, 3);
            
            // Act
            gameMatrix.SetBlockInt(0, 1, 2, 1);
            
            // Assert
            Assert.AreEqual(gameMatrix.Width, 3);
            Assert.AreEqual(gameMatrix.Height, 3);
            Assert.AreEqual(gameMatrix.Depth, 3);
            
            for (int i=0; i<gameMatrix.Width; i++)
            {
                for (int j=0; j<gameMatrix.Height; j++)
                {
                    for (int k=0; k<gameMatrix.Depth; k++)
                    {
                        if (i == 0 && j == 1 && k == 2)
                        {
                            Assert.AreEqual(gameMatrix.GetBlockInt(i, j, k), 1);
                        }
                        else
                        {
                            Assert.AreEqual(gameMatrix.GetBlockInt(i, j, k), -1);
                        }
                        Assert.AreEqual(gameMatrix.GetBlock(i, j, k), null);
                    }
                }
            }
        }
    }
}