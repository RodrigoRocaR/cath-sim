using System.Collections.Generic;
using Bots.Algorithms;
using Bots.DS.TreeModel;
using UnityEngine;

namespace Bots.DS.MonteCarlo
{
    public class State
    {
        private WallLevel2D _wallLevel2D;
        private Vector3 _playerPos;

        private WallHelper _wh;
        private BlockFrontier _blockFrontier;
        
        public State(Vector3 playerPos)
        {
            _wallLevel2D = new WallLevel2D(_wh);
            _playerPos = playerPos;
            
            _wh = new WallHelper(playerPos);
            _blockFrontier = new BlockFrontier(playerPos);
        }

        public State(State previous, PushPullAction action)
        {
            // todo; this
        }

        public int Evaluate()
        {
            return 0;
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
                //currNode.AddChild(state, action);
            }
        }


        private void LogErrorWrongNode()
        {
            Debug.LogError("When expanding the node does not contain the state desired to expand");
        }
    }
}