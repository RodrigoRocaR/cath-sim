using System.Collections.Generic;
using UnityEngine;

namespace Bots
{
    public class ActionStream
    {
        private List<Action> _actions;

        public ActionStream()
        {
            _actions = new List<Action>();
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
                _actions.Add(GetActionFromPosDiff(previousPos, positions[i]));
                previousPos = positions[i];
            }
        }

        private Action GetActionFromPosDiff((int, int) original, (int, int) target)
        {
            int zDiff = original.Item1 - target.Item1;
            switch (zDiff)
            {
                case -1:
                    return Action.Forward;
                case 1:
                    return Action.Backward;
            }

            int xDiff = original.Item2 - target.Item2;
            switch (xDiff)
            {
                case -1:
                    return Action.Right;
                case 1:
                    return Action.Left;
                default:
                    Debug.LogError("Failed to generate a movement action from positions");
                    return Action.Forward;
            }
        }
    }
}