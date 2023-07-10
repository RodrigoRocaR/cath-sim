using System.Collections.Generic;
using Blocks;
using Bots.Algorithms;
using Bots.DS.TreeModel;
using LevelDS;
using UnityEngine;

namespace Bots.DS.MonteCarlo
{
    public class State
    {
        private static System.Random _random = new System.Random();
        
        private GameMatrix _currentLevel;
        private Vector3 _playerPos;

        public int N { get; set; } // number of visits
        public int T { get; set; } // total score

        private BlockFrontier _blockFrontier;
        private List<PushPullAction> _possibleActions;
        
        private PushPullAction _excludedAction;

        public State(Vector3 playerPos) // root node constructor
        {
            _playerPos = Level.TransformToIndexDomain(playerPos);
            _currentLevel = Level.GetGameMatrix();
            _playerPos = playerPos;
            _blockFrontier = new BlockFrontier(playerPos, _currentLevel);
        }

        public State(State previous, PushPullAction action)
        {
            _playerPos = BlockHelper.GetNewPlayerPos(action);
            _currentLevel = new GameMatrix(previous._currentLevel, action);
            _blockFrontier = new BlockFrontier(_playerPos, _currentLevel);
            _excludedAction = new PushPullAction(PushPullAction.GetOppositeAction(action));
        }

        private int Evaluate()
        {
            int score = 0;
            
            if (_blockFrontier.ContainsBlocksWithZValue((int)_playerPos.z+2)) // reached goal
            {
                // It needs to be z+2 since it can always manipulate z+1 blocks from the wall
                score += 99;
            }
            else
            {
                score += _blockFrontier.Length();
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

            _possibleActions ??= PushPullAction.GetViableActions(_blockFrontier, _excludedAction);

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
            _possibleActions ??= PushPullAction.GetViableActions(_blockFrontier, _excludedAction);
            if (_possibleActions.Count == 0) return Evaluate();
            PushPullAction action;
            lock (_random)
            {
                action = _possibleActions[_random.Next(0, _possibleActions.Count)];
            }
            return new State(this, action).Rollout(depth - 1);
        }

        private void LogErrorWrongNode()
        {
            Debug.LogError("When expanding the node does not contain the state desired to expand");
        }
    }
}