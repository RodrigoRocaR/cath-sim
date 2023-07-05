using Bots.DS.MonteCarlo;
using Bots.DS.TreeModel;
using UnityEngine;

namespace Bots.Algorithms
{
    public class MonteCarlo
    {
        private TreeNode<State, PushPullAction> _searchTreeRoot;

        public MonteCarlo(Vector3 playerPos)
        {
            var wh = new WallHelper(playerPos);
            _searchTreeRoot = new TreeNode<State, PushPullAction>(new State(wh, playerPos));
        }

        public void Climb()
        {
            var root = _searchTreeRoot;
            while (true)
            {
                var nodeToRollout = TraverseAndExpand(root);
                int v = nodeToRollout.Value.Rollout();
                Backpropagate(v, nodeToRollout);
            }
        }

        private TreeNode<State, PushPullAction> TraverseAndExpand(TreeNode<State, PushPullAction> current)
        {
            if (!current.IsLeafNode())
            {
                float max = float.MinValue;
                foreach (var node in current.Forest)
                {
                    var score = UCB1(node);
                    if (max < score)
                    {
                        max = score;
                        current = node;
                        if (float.IsPositiveInfinity(max)) break;
                    }
                }
            }

            if (current.Value.N != 0)
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
    }
}