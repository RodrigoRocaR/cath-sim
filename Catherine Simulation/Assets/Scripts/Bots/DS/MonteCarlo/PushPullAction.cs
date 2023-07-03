using System.Collections.Generic;
using Blocks;
using Bots.Algorithms;
using LevelDS;
using UnityEngine;

namespace Bots.DS.MonteCarlo
{
    public class PushPullAction
    {
        private static BlockHelper _bh = new BlockHelper();

        public enum Actions
        {
            PushForward,
            PushRight,
            PushBackward,
            PushLeft,
            PullForward,
            PullRight,
            PullBackward,
            PullLeft,
        }

        public static Dictionary<Vector3, List<Actions>> GetViableActions(BlockFrontier bf)
        {
            Dictionary<Vector3, List<Actions>> viableActions = new Dictionary<Vector3, List<Actions>>();

            foreach (var blockPos in bf.GetFrontier())
            {
                viableActions.Add(blockPos, new List<Actions>());
                if (IsWalkable(_bh.Left(blockPos)))
                {
                    viableActions[blockPos].Add(Actions.PushRight);
                    if (Level.IsEmpty(_bh.Left(blockPos, multiplier: 2)))
                        viableActions[blockPos].Add(Actions.PullRight);
                }

                if (IsWalkable(_bh.Right(blockPos)))
                {
                    viableActions[blockPos].Add(Actions.PushLeft);
                    if (Level.IsEmpty(_bh.Right(blockPos, multiplier: 2)))
                        viableActions[blockPos].Add(Actions.PullLeft);
                }

                if (IsWalkable(_bh.Backward(blockPos)))
                {
                    viableActions[blockPos].Add(Actions.PushForward);
                    if (Level.IsEmpty(_bh.Backward(blockPos, multiplier: 2)))
                        viableActions[blockPos].Add(Actions.PullForward);
                }

                if (IsWalkable(_bh.Forward(blockPos)))
                {
                    viableActions[blockPos].Add(Actions.PushBackward);
                    if (Level.IsEmpty(_bh.Forward(blockPos, multiplier: 2)))
                        viableActions[blockPos].Add(Actions.PullBackward);
                }
            }

            return viableActions;
        }
        
        
        private static bool IsWalkable(Vector3 pos)
        {
            // IsEmpty and has a block below it
            return Level.IsEmpty(pos) && Level.IsNotEmpty(_bh.Down(pos));
        }
    }
}