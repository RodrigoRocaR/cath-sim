using System;
using System.Reflection;
using Bots.DS.MonteCarlo;
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
            Type type = typeof(State);
            MethodInfo methodInfo = type.GetMethod("Evaluate", BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo == null) Assert.Fail("Could not get Evaluate method");


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

            int score = Convert.ToInt32(methodInfo.Invoke(s, null));
            Assert.AreEqual(4, score);

            State s2 = new State(s, new PushPullAction(new Vector3(1, 1, 2), PushPullAction.Actions.PullForward));
            int score2 = Convert.ToInt32(methodInfo.Invoke(s2, null));
            Assert.Greater(score2, score);
        }
    }
}