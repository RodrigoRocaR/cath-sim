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
            get => _elements[x][y][z];
            set => _elements[x][y][z] = value;
        }
        
        public List<T> this[int x, int y]
        {
            get => _elements[x][y];
            set => _elements[x][y] = value;
        }
        
        public List<List<T>> this[int x]
        {
            get => _elements[x];
            set => _elements[x] = value;
        }

        public T this[Vector3Int coord]
        {
            get => _elements[coord.x][coord.y][coord.z];
            set => _elements[coord.x][coord.y][coord.z] = value;
        }

        public T this[Vector3 coord]
        {
            get
            {
                Vector3Int coordInt = Vector3Int.RoundToInt(coord);
                return _elements[coordInt.x][coordInt.y][coordInt.z];
            }

            set
            {
                Vector3Int coordInt = Vector3Int.RoundToInt(coord);
                _elements[coordInt.x][coordInt.y][coordInt.z] = value;
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
    }
}
