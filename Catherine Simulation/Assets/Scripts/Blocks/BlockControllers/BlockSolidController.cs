using Blocks.BlockTypes;

namespace Blocks.BlockControllers
{
    public class BlockSolidController : GenericBlockController
    {
        private BlockSolid _bs;
        private const float MoveDuration = 0.5f;

        private void Start()
        {
            if (_bs == null) InstantiateBlock();

        }

        private void Update()
        {
            _bs.UpdatePostionIfMoved();
        }
        
        public override IBlock GetBlockInstantiate()
        {
            InstantiateBlock();
            return _bs;
        }

        private void InstantiateBlock()
        {
            _bs = new BlockSolid(transform, MoveDuration);
        }
    }
}