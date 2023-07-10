using System.Collections;
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

        public MonteCarlo(Vector3 playerPos)
        {
            _searchTreeRoot = new TreeNode<State, PushPullAction>(new State(playerPos));
            BotEventManager.OnExplorationFinished += TriggerStopIterating;
        }

        public IEnumerator LookForClimbingRoutes()
        {
            var root = _searchTreeRoot;
            int i = 0;
            while (!_stopIterating && i < Parameters.MaxIterations)
            {
                var nodeToRollout = TraverseAndExpand(root);
                int v = nodeToRollout.Value.Rollout();
                Backpropagate(v, nodeToRollout);
                i++;
                yield return null;
            }

            if (i >= Parameters.MaxIterations)
            {
                Debug.LogWarning("MonteCarlo: I give up");
            }
            yield return null;
        }

        private TreeNode<State, PushPullAction> TraverseAndExpand(TreeNode<State, PushPullAction> current)
        {
            if (!current.IsLeafNode())
            {
                int childIndex = GetChildThatMaximizesUCB1(current);
                current = current.Forest[childIndex];
            }

            if (current.Value.N > 0)
            {
                return current.Value.Expand(current);
            }

            return current;
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
            // todo: transform into atomic actions
            return _actions;
        }
    }
}