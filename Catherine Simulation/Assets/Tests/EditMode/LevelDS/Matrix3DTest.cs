using System;
using LevelDS;
using NUnit.Framework;

namespace Tests.EditMode.LevelDS
{
    public class Matrix3DTest
    {
        [Test]
        public void IncreasesWidthByN()
        {
            // Arrange
            var matrix3D = new Matrix3D<int>(3, 3, 3, -1);
            int initialWidth = matrix3D.Width;
            int initialHeight = matrix3D.Height;
            int initialDepth = matrix3D.Depth;
            int n = 5;

            // Act
            matrix3D.IncreaseSize(0, n);

            // Assert
            Assert.AreEqual(initialWidth + n, matrix3D.Width);
            Assert.AreEqual(initialHeight, matrix3D.Height);
            Assert.AreEqual(initialDepth, matrix3D.Depth);
        }
        
        [Test]
        public void IncreasesHeightByN()
        {
            // Arrange
            var matrix3D = new Matrix3D<int>(3, 3, 3, -1);
            int initialWidth = matrix3D.Width;
            int initialHeight = matrix3D.Height;
            int initialDepth = matrix3D.Depth;
            int n = 5;

            // Act
            matrix3D.IncreaseSize(1, n);

            // Assert
            Assert.AreEqual(initialWidth, matrix3D.Width);
            Assert.AreEqual(initialHeight + n, matrix3D.Height);
            Assert.AreEqual(initialDepth, matrix3D.Depth);
        }
        
        [Test]
        public void IncreasesDepthByN()
        {
            // Arrange
            var matrix3D = new Matrix3D<int>(3, 3, 3, -1);
            int initialWidth = matrix3D.Width;
            int initialHeight = matrix3D.Height;
            int initialDepth = matrix3D.Depth;
            int n = 5;

            // Act
            matrix3D.IncreaseSize(2, n);

            // Assert
            Assert.AreEqual(initialWidth, matrix3D.Width);
            Assert.AreEqual(initialHeight, matrix3D.Height);
            Assert.AreEqual(initialDepth + n, matrix3D.Depth);
        }
        
        [Test]
        public void IncreasesNumberOfElementsInXAxis()
        {
            // Arrange
            var matrix3D = new Matrix3D<int>(3, 6, 9, -1);
            int initialWidth = matrix3D.Width;
            int n = 3;

            // Act
            matrix3D.IncreaseSize(0, n);

            // Assert
            for (int i=initialWidth; i<matrix3D.Width; i++)
            {
                Assert.AreEqual(matrix3D.Height, matrix3D[i].Count);
                for (int j=0; j<matrix3D.Height; j++)
                {
                    Assert.AreEqual(matrix3D.Depth, matrix3D[i][j].Count);
                    for (int k=0; k<matrix3D.Depth; k++)
                    {
                        Assert.AreEqual(-1, matrix3D[i][j][k]);
                    }
                }
            }
        }
        
        [Test]
        public void IncreasesNumberOfElementsInYAxis()
        {
            // Arrange
            var matrix3D = new Matrix3D<int>(3, 6, 9, -1);
            int initialHeight = matrix3D.Height;
            int n = 2;

            // Act
            matrix3D.IncreaseSize(1, n);

            // Assert
            for (int i=0; i<matrix3D.Width; i++)
            {
                Assert.AreEqual(matrix3D.Height, matrix3D[i].Count);
                for (int j=initialHeight; j<matrix3D.Height; j++)
                {
                    Assert.AreEqual(matrix3D.Depth, matrix3D[i][j].Count);
                    for (int k=0; k<matrix3D.Depth; k++)
                    {
                        Assert.AreEqual(-1, matrix3D[i][j][k]);
                    }
                }
            }
        }
        
        [Test]
        public void IncreasesNumberOfElementsInZAxis()
        {
            // Arrange
            var matrix3D = new Matrix3D<int>(3, 6, 9, -1);
            int initialDepth = matrix3D.Depth;
            int n = 4;

            // Act
            matrix3D.IncreaseSize(2, n);

            // Assert
            for (int i=0; i<matrix3D.Width; i++)
            {
                Assert.AreEqual(matrix3D.Height, matrix3D[i].Count);
                for (int j=0; j<matrix3D.Height; j++)
                {
                    Assert.AreEqual(matrix3D.Depth, matrix3D[i][j].Count);
                    for (int k=initialDepth; k<matrix3D.Depth; k++)
                    {
                        Assert.AreEqual(-1, matrix3D[i][j][k]);
                    }
                }
            }
        }

        [Test]
        public void ThrowsArgumentExceptionWhenAxisIsWrong()
        {
            // Arrange
            var obj = new Matrix3D<int>(10, 10, 10, 4);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => obj.IncreaseSize(6, 5));
            Assert.Throws<ArgumentException>(() => obj.IncreaseSize(-1, 5));
        }

        [Test]
        public void DoesNotModifyObjectWhenNIsZero()
        {
            // Arrange
            var obj = new Matrix3D<int>(3, 3, 3, -1);
            int initialWidth = obj.Width;

            // Act
            obj.IncreaseSize(0, 0);

            // Assert
            Assert.AreEqual(initialWidth, obj.Width);
        }
    }
}