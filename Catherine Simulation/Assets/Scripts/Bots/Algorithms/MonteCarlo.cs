using System.Collections.Generic;
using Bots.DS.MonteCarlo;
using Bots.DS.TreeModel;
using LevelDS;
using UnityEngine;

namespace Bots.Algorithms
{
    public class MonteCarlo
    {
        private TreeNode<State, PushPullAction> _searchTreeRoot;
        private List<PushPullAction> _actions;
        private bool _stopIterating;
        private TreeNode<State, PushPullAction> _terminalNode;

        public MonteCarlo(Vector3 playerPos)
        {
            if (playerPos.y > 55)
            {
                var a = 0;
            }

            _searchTreeRoot = new TreeNode<State, PushPullAction>(new State(playerPos));
            BotEventManager.OnExplorationFinished += TriggerStopIterating;
        }

        public void LookForClimbingRoutes()
        {
            var root = _searchTreeRoot;
            int i = 0;
            while (!_stopIterating && i < Parameters.MaxIterations)
            {
                var (nodeToRollout, isTerminal) = TraverseAndExpand(root);
                if (isTerminal)
                {
                    _terminalNode = nodeToRollout;
                    break;
                }

                int v = nodeToRollout.Value.Rollout();
                Backpropagate(v, nodeToRollout);
                i++;
            }

            if (i >= Parameters.MaxIterations)
            {
                Debug.LogWarning("MonteCarlo: I give up");
            }
            else if (!_stopIterating)
            {
                Debug.Log("Found a solution!");
            }
            else
            {
                Debug.Log("Ran out of time");
            }
        }

        private (TreeNode<State, PushPullAction>, bool) TraverseAndExpand(TreeNode<State, PushPullAction> current)
        {
            if (!current.IsLeafNode())
            { 
                current = GetChildThatMaximizesUCB1(current);
            }

            if (current.Value.N > 0 || current.IsRoot())
            {
                return current.Value.Expand(current);
            }

            return (current, current.Value.IsTerminal());
        }

        private void Backpropagate(int v, TreeNode<State, PushPullAction> node)
        {
            if (node == null) return;
            node.Value.N++;
            node.Value.T += v;
            Backpropagate(v, node.Parent);
        }

        private float UCB1(TreeNode<State, PushPullAction> node)
        {
            if (node.Value.N == 0)
            {
                return float.PositiveInfinity;
            }

            var explorationTerm = node.Value.T / node.Value.N;
            var exploitationTerm = Mathf.Sqrt(Mathf.Log(node.Parent.Value.N) / node.Value.N);
            return explorationTerm + Parameters.C * exploitationTerm;
        }

        private GameMatrix CollectBestCourseOfAction()
        {
            _actions = new List<PushPullAction>();
            if (_terminalNode != null)
            {
                return CollectFromTerminalNode();
            }

            var node = _searchTreeRoot;
            if (node == null)
            {
                Debug.LogError("root is null");
                return null;
            }

            var childNode = GetChildThatMaximizesUCB1(node, true);
            return childNode.Value.GetStateLevel();
        }

        private GameMatrix CollectFromTerminalNode()
        {
            var node = _terminalNode;
            while (!node.IsRoot())
            {
                _actions.Add(node.ParentEdge.Value);
                node = node.Parent;
            }

            _actions.Reverse();
            return _terminalNode.Value.GetStateLevel();
        }

        private TreeNode<State, PushPullAction> GetChildThatMaximizesUCB1(TreeNode<State, PushPullAction> exploreNode,
            bool collectMode = false)
        {
            if (exploreNode == null) return null;

            while (!exploreNode.IsLeafNode())
            {
                int ans = -1;
                float max = float.MinValue;
                for (int i = 0; i < exploreNode.Forest.Count; i++)
                {
                    var score = UCB1(exploreNode.Forest[i]);
                    if (max < score)
                    {
                        if (collectMode && float.IsPositiveInfinity(score)) continue;
                        max = score;
                        ans = i;
                        if (float.IsPositiveInfinity(max)) break;
                    }
                }

                if (collectMode)
                {
                    _actions.Add(exploreNode.Edges[ans].Value);
                }

                exploreNode = exploreNode.Forest[ans];
            }

            return exploreNode;
        }

        private void TriggerStopIterating()
        {
            _stopIterating = true;
        }

        public (List<PushPullAction>, GameMatrix) GetActions()
        {
            var currLevel = CollectBestCourseOfAction();
            return (_actions, currLevel);
        }
    }
}