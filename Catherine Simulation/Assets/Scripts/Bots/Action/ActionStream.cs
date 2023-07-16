using System.Collections.Generic;
using Blocks;
using Bots.DS;
using Bots.DS.MonteCarlo;
using LevelDS;
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
        private Level2D _level2D;

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

        public void AddActions(IEnumerable<Action> actions)
        {
            _actions.AddRange(actions);
        }


        /**
         * We assume the first position is the starting position and then we create n-1 actions to move to
         * the corresponding positions
        */
        public void CreateFromPositions(List<(int, int)> positions)
        {
            _positions = positions;
            var previousPos = _positions[0];
            int initialOffset = _actions.Count;
            for (int i = 1; i < _positions.Count; i++)
            {
                Action? a = GetActionFromPosDiff(previousPos, _positions[i]);
                if (a != null)
                {
                    _actions.Add((Action)a);
                }

                previousPos = _positions[i];
            }

            TranslateActionsTo3D(initialOffset);
        }


        public void CreateFromPushPullActions(Vector3 start, List<PushPullAction> pushPullActions)
        {
            Vector3 currPos = Level.TransformToIndexDomain(start);
            _positions.Add(((int)currPos.x, (int)currPos.z));
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
            TranslateActionsTo3D();
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

            int currHeight = _level2D.Get(currPos);
            for (int i = 0; i < diff; i++)
            {
                currPos = BlockHelper.GetNextPos(currPos, action);
                var nextHeight = _level2D.Get(currPos);

                if (Mathf.Abs(nextHeight - currHeight) > 1 || nextHeight == GameConstants.EmptyBlock)
                {
                    currPos = Hang(diff-i-1, action, currPos, currHeight);
                    break; // we go until the end
                }
                _actions.Add(action);
                _positions.Add(currPos);
                currHeight = nextHeight;
            }

            return new Vector3(currPos.Item1, _level2D.Get(currPos), currPos.Item2);
        }

        private void TranslateActionsTo3D(int initialOffset = 0)
        {
            if (_level2D.Width() == 0 || _level2D.Height() == 0) return;
            int currheightLevel = _level2D.Get(_positions[0].Item1, _positions[0].Item2);
            int offset = 0;
            for (int i = 1; i < _positions.Count; i++)
            {
                int newHeight = _level2D.Get(_positions[i]);
                if (newHeight != currheightLevel && newHeight != -1)
                {
                    int pushPullOffset = 0;
                    if (i+offset+1 < _actions.Count && _actions[i+offset+1] is Action.Push or Action.Pull) continue;
                    if (_actions[i+offset-1] is Action.Push) pushPullOffset = 1;
                    _actions.Insert(i + offset + initialOffset + pushPullOffset, Action.Jump);
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

        private (int, int) Hang(int diff, Action a, (int, int) currPos, int startheight)
        {
            _actions.Add(Action.Backward);
            _actions.Add(a);
            _positions.Add(PosBackward());
            while (diff > 0) // At this point we know we can perform the actions so we just go
            {
                _actions.Add(a);
                currPos = BlockHelper.GetNextPos(currPos, a);
                _positions.Add(PosBackward());
                
                // There are blocks in the way, so we have to hang on them instead: (hang backwards)
                while (_level2D.Get(PosBackward()) == startheight)
                {
                    _actions.Add(a);
                    currPos.Item2--;
                }
                
                // We dont have more blocks to hang, try to hang forward
                while (Level.GetBlockInt(currPos, startheight) == GameConstants.EmptyBlock)
                {
                    _actions.Add(a);
                    currPos.Item2++;
                }

                diff--;
            }

            _actions.Add(Action.Forward);
            _positions.Add(currPos);
            return currPos;

            (int, int) PosBackward()
            {
                return (currPos.Item1, currPos.Item2 - 1);
            }
        }
    }
}