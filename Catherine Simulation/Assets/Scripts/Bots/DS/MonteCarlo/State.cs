using Blocks;
using Bots.Algorithms;
using Bots.DS.TreeModel;
using UnityEngine;

namespace Bots.DS.MonteCarlo
{
    public class State
    {
        private WallLevel2D _wallLevel2D;
        private Vector3 _playerPos;
        
        private BlockFrontier _blockFrontier;
        
        public State(Vector3 playerPos)
        {
            _wallLevel2D = new WallLevel2D(new WallHelper(playerPos));
            _playerPos = playerPos;
            
            _blockFrontier = new BlockFrontier(playerPos);
        }

        public State(State previous, PushPullAction action)
        {
            _playerPos = BlockHelper.GetNewPlayerPos(previous._playerPos, action);
            _wallLevel2D = new WallLevel2D(previous._wallLevel2D, action);
            _blockFrontier = new BlockFrontier(_playerPos);
        }

        public int Evaluate()
        {
            var wallHelper = new WallHelper(_playerPos);
            int score = wallHelper.GetRelativeHeight();
            return score;
        }

        public void Expand(TreeNode<State, PushPullAction> currNode)
        {
            if (currNode.Value != this)
            {
                LogErrorWrongNode();
                return;
            }

            var possibleActions = PushPullAction.GetViableActions(_blockFrontier);

            foreach (var action in possibleActions)
            {
                currNode.AddChild(new State(this, action), action);
            }
        }

        private void LogErrorWrongNode()
        {
            Debug.LogError("When expanding the node does not contain the state desired to expand");
        }
    }
}