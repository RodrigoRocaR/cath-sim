using System.Collections.Generic;
using System.Threading.Tasks;
using Player;

namespace Bots
{
    public class ActionExecutor
    {
        private Queue<Action> _actionQueue;

        private readonly Dictionary<Action, int> _actionDelay = new Dictionary<Action, int>
        {
            { Action.Forward, 1000 },
            { Action.Backward, 1000 },
            { Action.Right, 1000 },
            { Action.Left, 1000 },
            { Action.Jump, 1000 },
            { Action.Push, 1000 },
            { Action.Pull, 1000 },
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

        private bool _done = true;
        private readonly Inputs _inputs;

        public ActionExecutor(Inputs inputs)
        {
            _inputs = inputs;
        }

        public async void ExecuteAll()
        {
            foreach (var action in _actionQueue)
            {
                _inputs.StartAction(action);
                await Task.Delay(_actionDelay[action]);
                _inputs.StopAction(action);
                await Task.Delay(_postActionDelay[action]);
            }

            _done = true;
        }

        public void Add(Action a)
        {
            _done = false;
            _actionQueue.Enqueue(a);
        }

        public void AddRange(List<Action> listActions)
        {
            if (listActions.Count == 0) return;
            _done = false;
            foreach (var action in listActions)
            {
                _actionQueue.Enqueue(action);
            }
        }

        public bool IsDone()
        {
            return _done;
        }
    }
}