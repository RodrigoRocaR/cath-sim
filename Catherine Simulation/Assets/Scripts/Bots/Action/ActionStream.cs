using System.Collections.Generic;
using Bots.DS;
using Bots.DS.MonteCarlo;
using LevelDS;
using UnityEngine;

namespace Bots.Action
{
    public class ActionStream
    {
        private List<Action> _actions;
        private readonly Level2D _level2D;

        public ActionStream(Level2D level2D)
        {
            _actions = new List<Action>();
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
            var previousPos = positions[0];
            for (int i = 1; i < positions.Count; i++)
            {
                Action? a = GetActionFromPosDiff(previousPos, positions[i]);
                if (a != null)
                {
                    _actions.Add((Action)a);
                }
                
                previousPos = positions[i];
            }

            TranslateActionsTo3D(positions);
        }

        public void TranslateActionsTo3D(List<(int, int)> positions)
        {
            if (_level2D.Width() == 0 || _level2D.Height() == 0) return;
            int currheightLevel = _level2D.Get(positions[0].Item1, positions[0].Item2);
            int offset = 0;
            for (int i=1; i<positions.Count; i++)
            {
                int newHeight = _level2D.Get(positions[i].Item1, positions[i].Item2);
                if (newHeight != currheightLevel)
                {
                    _actions.Insert(i+offset, Action.Jump);
                    currheightLevel = newHeight;
                    offset++;
                }
            }
        }

        private Action? GetActionFromPosDiff((int, int) original, (int, int) target)
        {
            return GetActionFromPosDiffX(original, target) ?? GetActionFromPosDiffZ(original, target);;
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