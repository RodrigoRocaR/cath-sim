using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelDS
{
    public class Matrix3D<T>
    {
        private List<List<List<T>>> _elements;
        private T _startValue;
        
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }

        public Matrix3D(int width, int height, int depth, T startValue)
        {
            Width = width;
            Height = height;
            Depth = depth;
            _startValue = startValue;
            _elements =  new List<List<List<T>>>();
            Initiliaze(_startValue);
        }
        
        public Matrix3D(int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;

            if (typeof(T) == typeof(int)) _startValue = (T)(object)GameConstants.EmptyBlock;
            else Debug.LogError("Cannot properly initialize Matrix due unknown start value");
            
            _elements =  new List<List<List<T>>>();
            Initiliaze(_startValue);
        }

        public Matrix3D(T[][][] values, T startValue)
        {
            _elements = ArrayToList(values);
            Width = _elements.Count;
            Height = _elements[0].Count;
            Depth = _elements[0][0].Count;
            _startValue = startValue;
        }
        
        public Matrix3D(T[][][] values)
        {
            _elements = ArrayToList(values);
            Width = _elements.Count;
            Height = _elements[0].Count;
            Depth = _elements[0][0].Count;
            
            if (typeof(T) == typeof(int)) _startValue = (T)(object)GameConstants.EmptyBlock;
            else Debug.LogError("Cannot properly initialize Matrix due unknown start value");
        }
        
        public Matrix3D(List<List<List<T>>> values)
        {
            _elements = values;
            Width = _elements.Count;
            Height = _elements[0].Count;
            Depth = _elements[0][0].Count;
            
            if (typeof(T) == typeof(int)) _startValue = (T)(object)GameConstants.EmptyBlock;
            else Debug.LogError("Cannot properly initialize Matrix due unknown start value");
        }

        public void IncreaseSize(int axis, int n)
        {
            if (n == 0) return;
            
            switch (axis)
            {
                case 0: // Increase size along the X axis
                    if (n > 0)
                    {
                        IncreaseSizeRight(startX: Width, endX: Width + n);
                        Width += n;
                    }
                    else
                    {
                        IncreaseSizeLeft(endX:-n);
                        Width -= n;
                    }
                    break;

                case 1: // Increase size along the Y axis
                    if (n > 0)
                    {
                        IncreaseSizeRight(startY:Height, endY:Height+n);
                        Height += n;
                    }
                    else
                    {
                        IncreaseSizeLeft(endY:-n);
                        Height -= n;
                    }
                    break;

                case 2: // Increase size along the Z axis
                    if (n > 0)
                    {
                        IncreaseSizeRight(startZ: Depth, endZ:Depth+n);
                        Depth += n;
                    }
                    else
                    {
                        IncreaseSizeLeft(endZ:-n);
                        Depth -= n;
                    }
                    break;
                
                default:
                    throw new ArgumentException();
            }
        }

        private void IncreaseSizeRight(int endX=0, int endY=0, int endZ=0, int startX=0, int startY=0, int startZ=0)
        {
            if (endX == 0) endX = Width;
            if (endY == 0) endY = Height;
            if (endZ == 0) endZ = Depth;

            bool expandingX = endX != Width, expandingY = endY != Height;
            

            for (int i=startX; i<endX; i++)
            {
                if (expandingX) _elements.Add(new List<List<T>>(Height));
                for (int j=startY; j<endY; j++)
                {
                    if (expandingX || expandingY) _elements[i].Add(new List<T>(Depth));
                    for (int k=startZ; k<endZ; k++)
                    {
                        _elements[i][j].Add(_startValue);
                    }
                }
            }
        }

        private void IncreaseSizeLeft(int startX = 0, int startY = 0, int startZ = 0, int endX = 0, int endY = 0, int endZ = 0)
        {
            if (endX == 0) endX = Width;
            if (endY == 0) endY = Height;
            if (endZ == 0) endZ = Depth;

            bool expandingX = endX != Width, expandingY = endY != Height;

            for (int i = endX - 1; i >= startX; i--)
            {
                if (expandingX) _elements.Insert(i, new List<List<T>>(Height));
                for (int j = startY; j < endY; j++)
                {
                    if (expandingX || expandingY) _elements[i].Insert(j, new List<T>(Depth));
                    for (int k = startZ; k < endZ; k++)
                    {
                        _elements[i][j].Insert(k, _startValue);
                    }
                }
            }
        }

        public T this[int x, int y, int z]
        {
            get =>  ValidCoords(x, y, z) ? _elements[x][y][z] : _startValue;
            set
            {
                if (ValidCoords(x, y, z)) _elements[x][y][z] = value;
            } 
        }
        
        public List<T> this[int x, int y]
        {
            get => ValidCoords(x, y) ? _elements[x][y] : new List<T>();
            set
            {
                if (ValidCoords(x, y)) _elements[x][y] = value;
            }
           
        }
        
        public List<List<T>> this[int x]
        {
            get => ValidCoords(x) ? _elements[x] : new List<List<T>>();
            set
            {
                if (ValidCoords(x)) _elements[x] = value;
            } 
        }

        public T this[Vector3Int coord]
        {
            get => ValidCoords(coord) ? _elements[coord.x][coord.y][coord.z] : _startValue;
            set
            {
                if (ValidCoords(coord)) _elements[coord.x][coord.y][coord.z] = value;
            } 
        }

        public T this[Vector3 coord]
        {
            get
            {
                Vector3Int coordInt = Vector3Int.RoundToInt(coord);
                return ValidCoords(coordInt) ? _elements[coordInt.x][coordInt.y][coordInt.z] : _startValue;
            }

            set
            {
                Vector3Int coordInt = Vector3Int.RoundToInt(coord);
                if (ValidCoords(coordInt)) _elements[coordInt.x][coordInt.y][coordInt.z] = value;
            }
        }

        private void Initiliaze(T startValue)
        {
            for (int i = 0; i < Width; i++)
            {
                _elements.Add(new List<List<T>>());
                for (int j = 0; j < Height; j++)
                {
                    _elements[i].Add(new List<T>());
                    for (int k = 0; k < Depth; k++)
                    {
                        _elements[i][j].Add(startValue);
                    }
                }
            }
        }

        private List<List<List<T>>> ArrayToList(T[][][] elems)
        {
            List<List<List<T>>> listOfLists = new List<List<List<T>>>();

            foreach (T[][] array2D in elems)
            {
                List<List<T>> innerList = new List<List<T>>();

                foreach (T[] array1D in array2D)
                {
                    List<T> innermostList = new List<T>(array1D);
                    innerList.Add(innermostList);
                }

                listOfLists.Add(innerList);
            }

            return listOfLists;
        }

        private bool ValidCoords(Vector3Int coords)
        {
            return ValidCoords(coords.x, coords.y, coords.z);
        }

        private bool ValidCoords(int x)
        {
            if (x >= Width)
            {
                LogOutOfBounds(x);
                return false;
            }

            return true;
        }
        
        private bool ValidCoords(int x, int y)
        {
            if (x >= Width || y >= Height)
            {
                LogOutOfBounds(x, y);
                return false;
            }

            return true;
        }

        private bool ValidCoords(int x, int y, int z)
        {
            if (x >= Width || y >= Height || z >= Depth)
            {
                LogOutOfBounds(x, y, z);
                return false;
            }
            return true;
        }

        private void LogOutOfBounds(int x, int y, int z)
        {
            Debug.LogError("Trying to get " + x + ", " + y + ", " + z + 
                           "; Dims: " + Width + ", " + Height  + ", " + Depth);
        }
        
        private void LogOutOfBounds(int x, int y)
        {
            Debug.LogError("Trying to get " + x + ", " + y + "; Dims: " + Width + ", " + Height);
        }
        
        private void LogOutOfBounds(int x)
        {
            Debug.LogError("Trying to get " + x + "; Dims: " + Width);
        }
        
        public Matrix3D<T> GetDeepCopy()
        {
            List<List<List<T>>> copy = new List<List<List<T>>>();

            foreach (List<List<T>> outerList in _elements)
            {
                List<List<T>> outerCopy = new List<List<T>>();

                foreach (List<T> innerList in outerList)
                {
                    List<T> innerCopy = new List<T>(innerList);
                    outerCopy.Add(innerCopy);
                }

                copy.Add(outerCopy);
            }

            return new Matrix3D<T>(copy);
        }
    }
}
