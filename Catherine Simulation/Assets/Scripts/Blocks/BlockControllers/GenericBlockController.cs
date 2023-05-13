using Blocks.BlockTypes;
using UnityEngine;

namespace Blocks.BlockControllers
{
    public abstract class GenericBlockController : MonoBehaviour
    {
        public IBlock GetBlockInstantiate()
        {
            return InstantiateBlock();
        }
        
        protected abstract IBlock InstantiateBlock();
    }
}