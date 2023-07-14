using System.Collections.Generic;

namespace Bots.DS.TreeModel
{
    public class TreeNode<T, K>
    {
        public List<Edge<T, K>> Edges { get; set; }
        public List<TreeNode<T, K>> Forest { get; set; }
        public TreeNode<T, K> Parent { get; set; }
        public Edge<T, K> ParentEdge { get; set; }
        public T Value { get; set; }

        public TreeNode(T value, TreeNode<T, K> parent)
        {
            Value = value;
            Forest = new List<TreeNode<T, K>>();
            Edges = new List<Edge<T, K>>();
            Parent = parent;
        }
        
        public TreeNode(T value)
        {
            // constructor for root nodes
            Value = value;
            Forest = new List<TreeNode<T, K>>();
            Edges = new List<Edge<T, K>>();
            Parent = null;
        }

        public void AddChild(T value, K edgevalue)
        {
            var childNode = new TreeNode<T, K>(value, this);
            var edge = new Edge<T, K>(this, childNode, edgevalue);
            Forest.Add(childNode);
            Edges.Add(edge);
            childNode.ParentEdge = edge;
        }

        public bool IsLeafNode()
        {
            return Forest.Count == 0;
        }

        public bool IsRoot()
        {
            return Parent == null;
        }
        
        
    }
}