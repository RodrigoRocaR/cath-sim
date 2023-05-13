using Blocks;
using Blocks.BlockControllers;
using Blocks.BlockTypes;
using LevelDS.LevelGen;
using UnityEngine;

namespace LevelDS
{
    public class Level : MonoBehaviour
    {
        public GameObject[] blockVariants;
        private readonly Vector3 _startCoords = new Vector3(0, 0.5f, 0);

        private static GameMatrix _level;
        private static LevelFactory _levelFactory;

        void Start()
        {
            _levelFactory = new LevelFactory();
            _level = _levelFactory.GetTestLevel();
            SpawnBlocks();
        }

        private void SpawnBlocks()
        {
            for (int i = 0; i < _level.Width; i++)
            {
                for (int j = 0; j < _level.Height; j++)
                {
                    for (int k = 0; k < _level.Depth; k++)
                    {
                        int blockInt = _level.GetBlockInt(i, j, k);
                        if (blockInt != GameConstants.EmptyBlock)
                        {
                            GameObject newBlock = Instantiate(blockVariants[blockInt],
                                new Vector3((_startCoords.x + i) * GameConstants.BlockScale,
                                    (_startCoords.y + j) * GameConstants.BlockScale,
                                    (_startCoords.z + k) * GameConstants.BlockScale),
                                blockVariants[blockInt].transform.rotation);

                            _level.SetBlock(i, j, k,
                                newBlock.GetComponent<GenericBlockController>().GetBlockInstantiate());
                        }
                    }
                }
            }
        }

        public static int GetBlockInt(Vector3 pos)
        {
            return _level.GetBlockInt(pos);
        }

        public static IBlock GetBlock(Vector3 pos)
        {
            return _level.GetBlock(pos);
        }

        public static bool IsBlock(Vector3 pos)
        {
            return _level.GetBlockInt(pos) != GameConstants.EmptyBlock;
        }

        public static void UpdateMovedBlock(Vector3 pos, Vector3 finalPos)
        {
            // Set up the new one
            _level.SetBlockInt(finalPos, GetBlockInt(pos));
            _level.SetBlock(finalPos, GetBlock(pos));

            // Erase old
            _level.SetBlockInt(pos, GameConstants.EmptyBlock);
            _level.SetBlock(pos, null);
        }
    }
}