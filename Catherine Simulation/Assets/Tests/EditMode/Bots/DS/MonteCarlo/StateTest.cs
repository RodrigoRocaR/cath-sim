using System;
using System.Reflection;
using Bots.DS.MonteCarlo;
using Bots.DS.TreeModel;
using LevelDS;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.Bots.DS.MonteCarlo
{
    public class StateTest
    {
        [Test]
        public void TestEvaluate()
        {
            GameObject mockObject = new GameObject();
            var mockLevel = mockObject.AddComponent<Level>();
            mockLevel.NewMockLevel(new[]
            {
                new[] // x: 0
                {
                    new[] { -1, 0, 0, -1 }, // y:0
                    new[] { -1, -1, 0, 0 }, // y:1
                    new[] { -1, -1, 0, 0 }, // y:2
                    new[] { -1, -1, -1, 0 }, // y:3
                },
                new[] // x: 1
                {
                    new[] { -1, 0, 0, -1 },
                    new[] { -1, -1, 0, 0 },
                    new[] { -1, -1, 0, 0 },
                    new[] { -1, -1, -1, 0 },
                },
                new[] // x: 2
                {
                    new[] { -1, 0, 0, -1 },
                    new[] { -1, -1, 0, 0 },
                    new[] { -1, -1, 0, 0 },
                    new[] { -1, -1, -1, 0 },
                },
                new[] // x: 3
                {
                    new[] { -1, 0, 0, -1 },
                    new[] { -1, -1, 0, 0 },
                    new[] { -1, -1, 0, 0 },
                    new[] { -1, -1, -1, 0 },
                },
            });
            State s = new State(new Vector3(0, 0, 1));
            
            var method = MethodGetter.GetPrivateMethod(s, "Evaluate");

            int score = Convert.ToInt32(method.Invoke(s, null));
            Assert.AreEqual(4, score);

            State s2 = new State(s, new PushPullAction(new Vector3(1, 1, 2), PushPullAction.Actions.PullForward));
            int score2 = Convert.ToInt32(method.Invoke(s2, null));
            Assert.Greater(score2, score);
        }

        [Test]
        public void TestExpand()
        {
            GameObject mockObject = new GameObject();
            var mockLevel = mockObject.AddComponent<Level>();
            mockLevel.NewMockLevel(new[]
            {
                new[] // x: 0
                {
                    new[] { -1, 0, 0, -1 },
                    new[] { -1, -1, 0, 0 }, 
                    new[] { -1, -1, 0, 0 }, 
                    new[] { -1, -1, -1, 0 }, 
                    new[] { -1, -1, -1, 0 }, 
                },
                new[] // x: 1
                {
                    new[] { -1, 0, 0, -1 },
                    new[] { -1, 0, 0, 0 },
                    new[] { -1, -1, 0, 0 },
                    new[] { -1, -1, 0, 0 },
                    new[] { -1, -1, -1, 0 }, 
                },
                new[] // x: 2
                {
                    new[] { -1, 0, 0, -1 },
                    new[] { -1, -1, 0, 0 },
                    new[] { -1, -1, 0, 0 },
                    new[] { -1, -1, -1, 0 },
                    new[] { -1, -1, -1, 0 }, 
                },
                new[] // x: 3
                {
                    new[] { -1, 0, 0, -1 },
                    new[] { -1, -1, 0, 0 },
                    new[] { -1, -1, 0, 0 },
                    new[] { -1, -1, -1, 0 },
                    new[] { -1, -1, -1, 0 }, 
                },
                new[] // x: 3
                {
                    new[] { -1, 0, 0, -1 },
                    new[] { -1, 0, 0, 0 },
                    new[] { -1, -1, 0, 0 },
                    new[] { -1, -1, 0, 0 },
                    new[] { -1, -1, -1, 0 }, 
                },
                new[] // x: 3
                {
                    new[] { -1, 0, 0, -1 },
                    new[] { -1, 0, 0, 0 },
                    new[] { -1, -1, 0, 0 },
                    new[] { -1, -1, 0, 0 },
                    new[] { -1, -1, -1, 0 }, 
                },
            });
            State s = new State(new Vector3(0, 0, 1));
            TreeNode<State, PushPullAction> root = new TreeNode<State, PushPullAction>(s);
            var firstChild = s.Expand(root);
            
            Assert.AreEqual(root.Forest[0], firstChild);
        }
    }
}