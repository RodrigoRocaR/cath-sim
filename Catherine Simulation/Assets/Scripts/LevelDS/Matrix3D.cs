using UnityEngine;

namespace LevelDS
{
    public class Matrix3D<T>
    {
        private T[,,] _blocks;
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }

        public Matrix3D(int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
            _blocks = new T[width, height, depth];
        }

        public T this[int x, int y, int z]
        {
            get => _blocks[x, y, z];
            set => _blocks[x, y, z] = value;
        }

        public T this[Vector3Int coord]
        {
            get => _blocks[coord.x, coord.y, coord.z];
            set => _blocks[coord.x, coord.y, coord.z] = value;
        }

        public T this[Vector3 coord]
        {
            get
            {
                Vector3Int coordInt = Vector3Int.RoundToInt(coord);
                return _blocks[coordInt.x, coordInt.y, coordInt.z];
            }

            set
            {
                Vector3Int coordInt = Vector3Int.RoundToInt(coord);
                _blocks[coordInt.x, coordInt.y, coordInt.z] = value;
            }
        }
    }
}
