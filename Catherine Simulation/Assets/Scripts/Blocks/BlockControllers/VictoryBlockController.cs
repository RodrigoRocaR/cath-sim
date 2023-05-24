using Blocks.BlockTypes;
using UnityEngine;

namespace Blocks.BlockControllers
{
    public class VictoryBlockController : GenericBlockController
    {
        public GameObject victoryCanvas;
        private GameObject _victoryCanvasInstance;
        private BlockVictory _bv;

        private void Start()
        {
            if (_bv == null) InstantiateBlock();
        }

        private void Update()
        {
        }

        protected override IBlock InstantiateBlock()
        {
            _victoryCanvasInstance = Instantiate(victoryCanvas);
            _victoryCanvasInstance.SetActive(false);
            _bv = new BlockVictory(_victoryCanvasInstance);
            return _bv;
        }
    }
}