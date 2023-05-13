using Blocks.BlockTypes;

namespace Blocks.BlockControllers
{
    public class VictoryBlockController : GenericBlockController
    {
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
            _bv = new BlockVictory();
            return _bv;
        }
    }
}