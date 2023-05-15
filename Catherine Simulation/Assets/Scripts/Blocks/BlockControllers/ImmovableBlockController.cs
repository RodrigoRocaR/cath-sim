using Blocks.BlockTypes;

namespace Blocks.BlockControllers
{
    public class ImmovableBlockController : GenericBlockController
    {
        private BlockImmovable _bi;
        
        private void Start()
        {
            if (_bi == null) InstantiateBlock();
        }
        
        protected override IBlock InstantiateBlock()
        {
            _bi = new BlockImmovable();
            return _bi;
        }
    }
}