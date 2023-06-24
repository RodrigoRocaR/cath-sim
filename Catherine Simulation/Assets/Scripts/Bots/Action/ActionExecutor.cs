using System.Collections.Generic;
using System.Threading;
using Player;

namespace Bots.Action
{
    public class ActionExecutor
    {
        private readonly Dictionary<Action, int> _actionDelay = new Dictionary<Action, int>
        {
            { Action.Forward, 100 },
            { Action.Backward, 100 },
            { Action.Right, 100 },
            { Action.Left, 100 },
            { Action.Jump, 100 },
            { Action.Push, 100 },
            { Action.Pull, 100 }
        };

        private readonly Dictionary<Action, int> _postActionDelay = new Dictionary<Action, int>
        {
            { Action.Forward, 1000 },
            { Action.Backward, 1000 },
            { Action.Right, 1000 },
            { Action.Left, 1000 },
            { Action.Jump, 1000 },
            { Action.Push, 1000 },
            { Action.Pull, 1000 },
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