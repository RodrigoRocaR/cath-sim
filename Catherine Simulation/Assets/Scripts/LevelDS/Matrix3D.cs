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
            Initiliaze(startValue);
        }

        public void IncreaseSize(int axis, int n)
        {
            if (n <= 0) return;
            
            switch (axis)
            {
                case 0: // Increase size along the X axis
                    for (int i=Width; i<Width+n; i++)
                    {
                        _elements.Add(new List<List<T>>(Height));
                        for (int j=0; j<Height; j++)
                        {
                            _elements[i].Add(new List<T>(Depth));
                            for (int k=0; k<Depth; k++)
                            {
                                _elements[i][j].Add(_startValue);
                            }
                        }
                    }
                    Width += n;
                    break;

                case 1: // Increase size along the Y axis
                    for (int i=0; i<Width; i++)
                    {
                        for (int j=Height; j<Height+n; j++)
                        {
                            _elements[i].Add(new List<T>(Depth));
                            for (int k=0; k<Depth; k++)
                            {
                                _elements[i][j].Add(_startValue);
                            }
                        }
                    }

                    Height += n;
                    break;

                case 2: // Increase size along the Z axis
                    for (int i=0; i<Width; i++)
                    {
                        for (int j=0; j<Height; j++)
                        {
                            for (int k=Depth; k<Depth+n; k++)
                            {
                                _elements[i][j].Add(_startValue);
                            }
                        }
                    }

                    Depth += n;
                    break;
                
                default:
                    throw new ArgumentException();
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
        
    }
}
