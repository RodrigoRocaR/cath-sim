using System;
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

        public Vector3 BlockPos { set; get; }
        public Actions Action { set; get; }

        public PushPullAction(Vector3 blockPos, Actions action)
        {
            BlockPos = blockPos;
            Action = action;
        }

        public PushPullAction(Actions action)
        {
            BlockPos = new Vector3();
            Action = action;
        }

        public bool IsAxisXMove()
        {
            return Action is Actions.PushRight or Actions.PushLeft or Actions.PullRight or Actions.PullLeft;
        }

        public bool IsLeftMove()
        {
            return Action is Actions.PushRight or Actions.PullLeft;
        }

        public bool IsRightMove()
        {
            return Action is Actions.PushLeft or Actions.PullRight;
        }

        public bool IsForwardMove()
        {
            return Action is Actions.PushForward or Actions.PullBackward;
        }

        public bool IsBackwardMove()
        {
            return Action is Actions.PushBackward or Actions.PullForward;
        }

        public bool IsPushAction()
        {
            return Action is Actions.PushBackward or Actions.PushForward or Actions.PushLeft or Actions.PushRight;
        }

        public static Actions GetOppositeAction(PushPullAction pushPullAction)
        {
            var a = pushPullAction.Action;
            return a switch
            {
                Actions.PushForward => Actions.PullForward,
                Actions.PushRight => Actions.PullRight,
                Actions.PushBackward => Actions.PullBackward,
                Actions.PushLeft => Actions.PullLeft,
                Actions.PullForward => Actions.PushForward,
                Actions.PullRight => Actions.PushRight,
                Actions.PullBackward => Actions.PushBackward,
                Actions.PullLeft => Actions.PushLeft,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static bool AreSameAction(Actions? a1, PushPullAction a2)
        {
            if (a2 == null || a1 == null)
            {
                return false;
            }

            return a1 == a2.Action;
        }

        public static List<PushPullAction> GetViableActions(BlockFrontier bf, PushPullAction excludedAction = null)
        {
            var actionDict = GetViableActionsAsDict(bf);

            List<PushPullAction> ans = new List<PushPullAction>();

            foreach (var (blockPos, actionsList) in actionDict)
            {
                foreach (var action in actionsList)
                {
                    if (!AreSameAction(action, excludedAction))
                        ans.Add(new PushPullAction(blockPos, action));
                }
            }

            return ans;
        }

        public static Dictionary<Vector3, List<Actions>> GetViableActionsAsDict(BlockFrontier bf)
        {
            Dictionary<Vector3, List<Actions>> viableActions = new Dictionary<Vector3, List<Actions>>();

            foreach (var blockPos in bf.GetFrontier())
            {
                if (!Level.CanBeMoved(blockPos)) continue;
                viableActions.Add(blockPos, new List<Actions>());
                if (IsWalkable(_bh.Left(blockPos)))
                {
                    if (Level.IsEmpty(_bh.Right(blockPos))) viableActions[blockPos].Add(Actions.PushRight);
                    if (Level.IsEmpty(_bh.Left(blockPos, multiplier: 2)))
                        viableActions[blockPos].Add(Actions.PullRight);
                }

                if (IsWalkable(_bh.Right(blockPos)))
                {
                    if (Level.IsEmpty(_bh.Left(blockPos))) viableActions[blockPos].Add(Actions.PushLeft);
                    if (Level.IsEmpty(_bh.Right(blockPos, multiplier: 2)))
                        viableActions[blockPos].Add(Actions.PullLeft);
                }

                if (IsWalkable(_bh.Backward(blockPos)))
                {
                    if (Level.IsEmpty(_bh.Forward(blockPos))) viableActions[blockPos].Add(Actions.PushForward);
                    if (Level.IsEmpty(_bh.Backward(blockPos, multiplier: 2)))
                        viableActions[blockPos].Add(Actions.PullForward);
                }

                if (IsWalkable(_bh.Forward(blockPos)))
                {
                    if (Level.IsEmpty(_bh.Backward(blockPos))) viableActions[blockPos].Add(Actions.PushBackward);
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

        public Action.Action GetAction()
        {
            return IsPushAction() ? Bots.Action.Action.Push : Bots.Action.Action.Pull;
        }
    }
}