using System.Collections.Generic;
using Bots.DS.MonteCarlo;
using Bots.DS.TreeModel;
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
            if (playerPos.y > 45)
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
                int childIndex = GetChildThatMaximizesUCB1(current);
                current = current.Forest[childIndex];
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

        private void CollectBestCourseOfAction()
        {
            _actions = new List<PushPullAction>();
            if (_terminalNode != null)
            {
                CollectFromTerminalNode();
                return;
            }
            
            var node = _searchTreeRoot;
            if (node == null) return;
            
            while (!node.IsLeafNode())
            {
                var childIndex = GetChildThatMaximizesUCB1(node, true);
                if (childIndex == -1)
                { // all child nodes are unexplored so it is like a leaf node
                    break;
                }

                _actions.Add(node.Edges[childIndex].Value);
                node = node.Forest[childIndex];
            }
        }

        private void CollectFromTerminalNode()
        {
            var node = _terminalNode;
            while (!node.IsRoot())
            {
                _actions.Add(node.ParentEdge.Value);
                node = node.Parent;
            }
            _actions.Reverse();
        }
        
        private int GetChildThatMaximizesUCB1(TreeNode<State, PushPullAction> exploreNode, bool collectMode = false)
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

            return ans;
        }

        private void TriggerStopIterating()
        {
            _stopIterating = true;
        }

        public List<PushPullAction> GetActions()
        {
            CollectBestCourseOfAction();
            return _actions;
        }
    }
}