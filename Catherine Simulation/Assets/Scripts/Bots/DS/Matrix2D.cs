using UnityEngine;

namespace Bots.DS
{
    public class Matrix2D<T>
    {
        private T[][] _elements;
        private T _startValue;
        private int _width;
        private int _height;

        public Matrix2D(int width, int height, T startValue)
        {
            _width = width;
            _height = height;
            _startValue = startValue;
            Initialize();
        }

        public Matrix2D(T[][] elements)
        {
            _elements = elements;
            _height = _elements.Length;
            _width = elements[0].Length;
            InferStartValueAsEmptyBlock();
        }

        public Matrix2D(int width, int height)
        {
            _width = width;
            _height = height;
            InferStartValueAsEmptyBlock();
            Initialize();
        }

        private void Initialize()
        {
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    _elements[i][j] = _startValue;
                }
            }
        }


        public T this[int x, int y]
        {
            get => _elements[x][y];
            set => _elements[x][y] = value;
        }

        public int GetWidth()
        {
            return _width;
        }

        public int GetHeight()
        {
            return _height;
        }

        private void InferStartValueAsEmptyBlock()
        {
            if (typeof(T) == typeof(int))
            {
                _startValue = (T)(object)GameConstants.EmptyBlock;
            }
            else
            {
                Debug.LogError("Cannot properly initialize Matrix due unknown start value");
            }
        }
    }
}