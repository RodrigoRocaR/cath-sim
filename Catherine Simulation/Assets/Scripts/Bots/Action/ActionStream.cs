using System.Collections.Generic;
using Blocks;
using Bots.DS;
using Bots.DS.MonteCarlo;
using UnityEngine;

namespace Bots.Action
{
    public class ActionStream
    {
        private enum Axis
        {
            X,
            Z
        }

        private List<Action> _actions;
        private List<(int, int)> _positions;
        private readonly Level2D _level2D;

        public ActionStream(Level2D level2D)
        {
            _actions = new List<Action>();
            _positions = new List<(int, int)>();
            _level2D = level2D;
        }

        public List<Action> GetAsList()
        {
            return _actions;
        }


        /**
         * We assume the first position is the starting position and then we create n-1 actions to move to
         * the corresponding positions
        */
        public void CreateFromPositions(List<(int, int)> positions)
        {
            _positions = positions;
            var previousPos = _positions[0];
            for (int i = 1; i < _positions.Count; i++)
            {
                Action? a = GetActionFromPosDiff(previousPos, _positions[i]);
                if (a != null)
                {
                    _actions.Add((Action)a);
                }

                previousPos = _positions[i];
            }

            TranslateActionsTo3D();
        }


        public void CreateFromPushPullActions(Vector3 start, List<PushPullAction> pushPullActions)
        {
            Vector3 currPos = start;
            foreach (var pushPullAction in pushPullActions)
            {
                var nextPos = BlockHelper.GetExpectedPlayerPos(pushPullAction);
                // 1. Move one or several blocks to needed pos
                currPos = MoveTorwardsPosAxis(currPos, nextPos, Axis.X);
                currPos = MoveTorwardsPosAxis(currPos, nextPos, Axis.Z);
                // 2. Look at block
                _actions.Add(BlockHelper.GetActionToLookTorwardsBlock(pushPullAction));
                // 3. Push / Pull it
                _actions.Add(pushPullAction.GetAction());
            }
        }

        private Vector3 MoveTorwardsPosAxis(Vector3 current, Vector3 target, Axis axis)
        {
            int diff = axis switch
            {
                Axis.X => (int)Mathf.Abs(current.x - target.x),
                Axis.Z => (int)Mathf.Abs(current.z - target.z),
                _ => 0
            };
            Action? actionNullable = GetActionFromPosDiff(current, target);
            if (actionNullable == null) return current;

            Action action = (Action)actionNullable;
            (int, int) currPos = ((int)current.x, (int)current.z);
            
            // todo: add hanging logic here
            for (int i=0; i<diff; i++)
            {
                _actions.Add(action);
                currPos = BlockHelper.GetNextPos(currPos, action);
                _positions.Add(currPos);
            }

            return new Vector3(currPos.Item1, _level2D.Get(currPos), currPos.Item2);
        }

        private void TranslateActionsTo3D()
        {
            if (_level2D.Width() == 0 || _level2D.Height() == 0) return;
            int currheightLevel = _level2D.Get(_positions[0].Item1, _positions[0].Item2);
            int offset = 0;
            for (int i = 1; i < _positions.Count; i++)
            {
                int newHeight = _level2D.Get(_positions[i].Item1, _positions[i].Item2);
                if (newHeight != currheightLevel)
                {
                    _actions.Insert(i + offset, Action.Jump);
                    currheightLevel = newHeight;
                    offset++;
                }
            }
        }

        private Action? GetActionFromPosDiff(Vector3 original, Vector3 target)
        {
            return GetActionFromPosDiff(((int)original.x, (int)original.z), ((int)target.x, (int)target.z));
        }

        private Action? GetActionFromPosDiff((int, int) original, (int, int) target)
        {
            return GetActionFromPosDiffX(original, target) ?? GetActionFromPosDiffZ(original, target);
        }

        private Action? GetActionFromPosDiffZ((int, int) original, (int, int) target)
        {
            int zDiff = original.Item2 - target.Item2;
            return zDiff switch
            {
                < 0 => Action.Forward,
                > 0 => Action.Backward,
                _ => null
            };
        }

        private Action? GetActionFromPosDiffX((int, int) original, (int, int) target)
        {
            int xDiff = original.Item1 - target.Item1;
            return xDiff switch
            {
                < 0 => Action.Right,
                > 0 => Action.Left,
                _ => null
            };
        }
    }
}