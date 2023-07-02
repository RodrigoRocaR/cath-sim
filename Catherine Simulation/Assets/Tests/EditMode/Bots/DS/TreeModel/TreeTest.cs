using System.Collections.Generic;
using Bots.DS.TreeModel;
using NUnit.Framework;

namespace Tests.EditMode.Bots.DS.TreeModel
{
    public class TreeTest
    {
        [Test]
        public void TestForestAndEdgesCorrelate()
        {
            //  1        \\\   2
            // 8 - 3 - 7 \\\  4 - 5
            TreeNode<int, string> root = new TreeNode<int, string>(-1);
            root.AddChild(1, "a1");
            root.AddChild(2, "a2");
            var left = root.Forest[0];
            left.AddChild(8, "a3");
            left.AddChild(3, "a4");
            left.AddChild(7, "a5");
            var right = root.Forest[1];
            right.AddChild(4, "a6");
            right.AddChild(5, "a7");

            Dictionary<int, string> expectedEdge = new Dictionary<int, string>
            {
                {1, "a1"},
                {2, "a2"},
                {3, "a4"},
                {4, "a6"},
                {5, "a7"},
                {7, "a5"},
                {8, "a3"},
            };

            for (int i=0; i<root.Forest.Count; i++)
            {
                var node = root.Forest[i];
                Assert.AreEqual(expectedEdge[node.Value], root.Edges[i].Value);
                for (int j=0; j<node.Forest.Count; j++)
                {
                    var childNode = node.Forest[j];
                    Assert.AreEqual(expectedEdge[childNode.Value], node.Edges[j].Value);;
                }
            }
        }
    }
}