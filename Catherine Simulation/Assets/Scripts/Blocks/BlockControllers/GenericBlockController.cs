using Blocks.BlockTypes;
using UnityEngine;

namespace Blocks.BlockControllers
{
    public abstract class GenericBlockController : MonoBehaviour
    {
        public abstract IBlock GetBlockInstantiate();
    }
}