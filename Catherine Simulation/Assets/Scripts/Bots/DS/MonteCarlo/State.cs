using System.Collections.Generic;
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

        public int N { get; set; } // number of visits
        public int T { get; set; } // total score

        private BlockFrontier _blockFrontier;
        private List<PushPullAction> _possibleActions;

        private WallHelper _wallHelper;

        public State(WallHelper wh, Vector3 playerPos)
        {
            _wallHelper = wh;
            _wallLevel2D = new WallLevel2D(wh);
            _playerPos = playerPos;

            _blockFrontier = new BlockFrontier(playerPos);
        }

        private State(State previous, PushPullAction action)
        {
            _playerPos = BlockHelper.GetNewPlayerPos(previous._playerPos, action);
            _wallLevel2D = new WallLevel2D(previous._wallLevel2D, action);
            _blockFrontier = new BlockFrontier(_playerPos);
        }

        private int Evaluate()
        {
            int score = 0;
            if ((int)_playerPos.z >= _wallHelper.GetTargetZ())
            {
                score += 99;
            }
            else
            {
                score += _wallHelper.GetRelativeHeight();
            }

            return score;
        }

        public TreeNode<State, PushPullAction> Expand(TreeNode<State, PushPullAction> currNode)
        {
            if (currNode.Value != this)
            {
                LogErrorWrongNode();
                return null;
            }

            _possibleActions ??= PushPullAction.GetViableActions(_blockFrontier);

            foreach (var action in _possibleActions)
            {
                currNode.AddChild(new State(this, action), action);
            }

            return currNode.Forest[0];
        }

        public int Rollout()
        {
            return Rollout(Parameters.RolloutDepth);
        }

        private int Rollout(int depth)
        {
            if (depth == 0) return Evaluate();
            _possibleActions ??= PushPullAction.GetViableActions(_blockFrontier);
            var action = _possibleActions[Random.Range(0, _possibleActions.Count)];
            return new State(this, action).Rollout(depth - 1);
        }

        private void LogErrorWrongNode()
        {
            Debug.LogError("When expanding the node does not contain the state desired to expand");
        }
    }
}