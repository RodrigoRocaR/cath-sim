using Blocks.BlockControllers;
using Blocks.BlockTypes;
using LevelDS.LevelGen;
using Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelDS
{
    public class Level : MonoBehaviour
    {
        public GameObject[] blockVariants;
        public GameObject player;
        public PlayerIdentity playerIdentityValue;
        public static PlayerIdentity PlayerIdentity;

        private readonly Vector3 _startCoords = new Vector3(0, 0.5f, 0);

        private static bool _isCleared, _isGameOver;
        private static GameMatrix _level;
        private static LevelFactory _levelFactory;

        private static bool _isMock;

        void Start()
        {
            AssignPlayerIdentity(playerIdentityValue);
            CharacterMaterialSwitcher.Switch(player, PlayerIdentity);
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName != "TestScene")
            {
                _level = new GameMatrix(50, 250, 250);
                GetBlocksFromScene();
            }
            else
            {
                _levelFactory = new LevelFactory();
                _level = _levelFactory.GetTestHangingLevel();
                SpawnBlocks();
            }

            SetPlayerInitialPosition(sceneName);
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

        private void GetBlocksFromScene()
        {
            GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Block");
            foreach (var block in allBlocks)
            {
                Vector3 pos = block.transform.position;
                if (block.name.Contains(GameConstants.SolidBlockName))
                {
                    _level.SetBlockInt(pos, GameConstants.SolidBlock);
                }
                else if (block.name.Contains(GameConstants.ImmovableBlockName))
                {
                    _level.SetBlockInt(pos, GameConstants.ImmovableBlock);
                }
                else if (block.name.Contains(GameConstants.VictoryBlockName))
                {
                    _level.SetBlockInt(pos, GameConstants.VictoryBlock);
                }

                _level.SetBlock(pos, block.GetComponent<GenericBlockController>().GetBlockInstantiate());
            }
        }

        public void NewMockLevel(int[][][] values)
        {
            _isMock = true;
            _level = new GameMatrix(values.Length, values[0].Length, values[0][0].Length, true);
            for (int i = 0; i < _level.Width; i++)
            {
                for (int j = 0; j < _level.Height; j++)
                {
                    for (int k = 0; k < _level.Depth; k++)
                    {
                        _level.SetBlockInt(i, j, k, values[i][j][k]);
                    }
                }
            }
        }

        private void SetPlayerInitialPosition(string sceneName)
        {
            if (GameConstants.PlayerInitialPosition.ContainsKey(sceneName))
            {
                player.transform.position = GameConstants.PlayerInitialPosition[sceneName];
            }
        }

        public static int GetBlockInt(int x, int y, int z)
        {
            return _level.GetBlockInt(x, y, z);
        }
        
        public static int GetBlockInt((int, int) xz, int y)
        {
            return _level.GetBlockInt(xz.Item1, y, xz.Item2);
        }

        public static int GetBlockInt(Vector3 pos)
        {
            return _level.GetBlockInt(pos);
        }

        public static bool IsEmpty(Vector3 pos)
        {
            return _level.IsEmpty(pos);
        }

        public static bool IsEmpty(int x, int y, int z)
        {
            return _level.IsEmpty(x, y, z);
        }

        public static bool IsNotEmpty(Vector3 pos)
        {
            return !IsEmpty(pos);
        }

        public static bool IsNotEmpty(int x, int y, int z)
        {
            return !IsEmpty(x, y, z);
        }

        public static IBlock GetBlock(Vector3 pos)
        {
            return _level.GetBlock(pos);
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
        
        public static bool IsBlock(Vector3 pos)
        {
            return _level.GetBlockInt(pos) != GameConstants.EmptyBlock;
        }

        public static bool IsInBlockSpaceCoords(Vector3 playerPos)
        {
            Vector3 slotPos = new Vector3(playerPos.x % GameConstants.BlockScale,
                playerPos.y % GameConstants.BlockScale,
                playerPos.z % GameConstants.BlockScale);
            const float closeToBlock = GameConstants.BlockScale - 0.01f;

            return
                slotPos.x is 0 or >= closeToBlock &&
                slotPos.y is 0 or >= closeToBlock &&
                slotPos.z is 0 or >= closeToBlock;
        }

        public static bool IsCleared()
        {
            return _isCleared;
        }

        public static void Finish()
        {
            _isCleared = true;
        }

        public static void GameOver()
        {
            _isGameOver = true;
        }

        public static bool IsGameOver()
        {
            return _isGameOver;
        }

        public static PlayerIdentity GetPlayerIdentity()
        {
            return PlayerIdentity;
        }

        public static Matrix3D<int> GetLevelAsMatrixInt()
        {
            return _level?.GetMatrixInt();
        }

        public static Matrix3D<IBlock> GetLevelAsMatrixBlocks()
        {
            return _level?.GetMatrixBlocks();
        }

        private static void AssignPlayerIdentity(PlayerIdentity playerIdentity)
        {
            PlayerIdentity = playerIdentity;
        }

        public static bool IsNull()
        {
            return _level == null;
        }

        public static bool IsMock()
        {
            return _isMock;
        }

        public static int Width()
        {
            return _level.Width;
        }

        public static int Height()
        {
            return _level.Height;
        }

        public static int Depth()
        {
            return _level.Depth;
        }

        public static Vector3 TransformToIndexDomain(Vector3 vector3)
        {
            if (_isMock) return vector3;
            return new Vector3((int) (vector3.x / GameConstants.BlockScale),
                (int) (vector3.y / GameConstants.BlockScale),
                (int) (vector3.z / GameConstants.BlockScale));
        }
        
        public static (int, int, int) TransformToIndexDomainAsTuple(Vector3 vector3)
        {
            return ((int) (vector3.x / GameConstants.BlockScale),
                (int) (vector3.y / GameConstants.BlockScale),
                (int) (vector3.z / GameConstants.BlockScale));
        }

        public static GameMatrix GetGameMatrix()
        {
            return _level;
        }
    }
}