namespace Bots.DS.TreeModel
{
    public class Edge<T, K>
    {
        public TreeNode<T, K> ParentNode { get; set; }
        public TreeNode<T, K> ChildNode { get; set; }
        public K Value { get; set; }

        public Edge(TreeNode<T, K> parentNode, TreeNode<T, K> childNode, K value)
        {
            ParentNode = parentNode;
            ChildNode = childNode;
            Value = value;
        }
    }
}