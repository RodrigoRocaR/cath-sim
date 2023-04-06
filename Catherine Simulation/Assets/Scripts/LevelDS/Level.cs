using UnityEngine;

namespace LevelDS
{
    public class Level : MonoBehaviour
    {
        public GameObject[] blockVariants;
        public const int EmptyBlock = -1;
        public const int SolidBlock = 0;
        public const int LevelSize = 12;
        public const int BlockScale = 2;
    
        public readonly Vector3 startCoords = new Vector3(0, 0.5f, 0);

        private static GameMatrix _level;

        void Start()
        {
            _level = new GameMatrix(LevelSize, LevelSize, LevelSize);
            InitializeLevel();
            GetTestLevel();
            SpawnBlocks();
        }

        void Update()
        {
        
        }

        private void SpawnBlocks()
        {
            for (int i=0; i<_level.Width; i++)
            {
                for (int j=0; j<_level.Height; j++)
                {
                    for (int k=0; k<_level.Depth; k++)
                    {
                        int blockInt = _level.GetBlockInt(i, j, k);
                        if (blockInt != EmptyBlock)
                        {
                            GameObject newBlock = Instantiate(blockVariants[blockInt],
                                new Vector3((startCoords.x + i) * BlockScale,
                                    (startCoords.y + j) * BlockScale,
                                    (startCoords.z + k) * BlockScale),
                                blockVariants[blockInt].transform.rotation);
                            _level.SetBlock(i, j, k, newBlock);
                        }
                    }
                }
            }
        }

        private void InitializeLevel()
        {
            for (int i=0; i<LevelSize; i++)
            {
                for (int j=0; j<LevelSize; j++)
                {
                    for (int k=0; k<LevelSize; k++)
                    {
                        _level.SetBlockInt(i, j, k, EmptyBlock);
                    }
                }
            }
        }

        private void GetTestLevel()
        {
            AddPlatform(0);
            AddWall(1, LevelSize-1, 3);
            AddWall(1, LevelSize-1, 3, false);
            AddWall(1, LevelSize-5, 1, false);
            AddWall(1, 0, 3, false);
            _level.SetBlockInt(1, 1, 3, SolidBlock);
        }
    
        private void AddPlatform(int y)
        {
            for (int j=0; j<LevelSize; j++)
            {
                for (int k=0; k<LevelSize; k++)
                {
                    _level.SetBlockInt(j, y, k, 0);
                }
            }
        }

        private void AddWall(int y,  int pos, int height = 2, bool horizontal = true)
        {
            for (int h=0; h<height; h++)
            {
                for (int k=0; k<LevelSize; k++)
                {
                    if (horizontal)
                    {
                        _level.SetBlockInt(k, y+h, pos, SolidBlock);
                    }
                    else
                    {
                        _level.SetBlockInt(pos, y+h, k, SolidBlock);
                    }
                }
            }
        }

        public static int GetBlockInt(Vector3 pos)
        {
            return _level.GetBlockInt(pos);
        }

        public static GameObject GetBlock(Vector3 pos)
        {
            return _level.GetBlock(pos);
        }
    }
}
