using Blocks.BlockTypes;

namespace Blocks.BlockControllers
{
    public class SolidBlockController : GenericBlockController
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

        protected override IBlock InstantiateBlock()
        {
            _bs = new BlockSolid(transform, MoveDuration);
            return _bs;
        }
    }
}