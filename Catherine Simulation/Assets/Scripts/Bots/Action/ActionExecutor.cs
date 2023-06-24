using System.Collections.Generic;
using System.Threading;
using Player;

namespace Bots.Action
{
    public class ActionExecutor
    {
        private readonly Dictionary<Action, int> _actionDelay = new Dictionary<Action, int>
        {
            { Action.Forward, 3_000 },
            { Action.Backward, 3_000 },
            { Action.Right, 3_000 },
            { Action.Left, 3_000 },
            { Action.Jump, 3_000 },
            { Action.Push, 3_000 },
            { Action.Pull, 3_000 }
        };

        private readonly Dictionary<Action, int> _postActionDelay = new Dictionary<Action, int>
        {
            { Action.Forward, 200 },
            { Action.Backward, 200 },
            { Action.Right, 200 },
            { Action.Left, 200 },
            { Action.Jump, 200 },
            { Action.Push, 200 },
            { Action.Pull, 200 },
        };

        private readonly Inputs _inputs;

        public ActionExecutor(Inputs inputs)
        {
            _inputs = inputs;
        }

        /**
         * Should be called as a  separate thread
         */
        public void Execute(object actionStreamObject)
        {
            // Cast the parameter to its appropriate type
            ActionStream actionStream = (ActionStream)actionStreamObject;

            foreach (var action in actionStream.GetAsList())
            {
                _inputs.StartAction(action);
                Thread.Sleep(_actionDelay[action]);
                _inputs.StopAction(action);
                Thread.Sleep(_postActionDelay[action]);
            }
        }
    }
}