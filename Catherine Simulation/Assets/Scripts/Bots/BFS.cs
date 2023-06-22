using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bots.Action;
using Bots.DS;

namespace Bots
{
    // ReSharper disable once InconsistentNaming
    public class BFS
    {
        private Queue<(int, int)> _unvisited;
        private HashSet<(int, int)> _visited;
        private Dictionary<(int, int), ((int, int), int)> _pathToPos; // pos_wanted : (pos_previous, length_of_path)

        private ActionStream _actionStream;
        private Level2D _level2D;

        public BFS(Level2D level2D)
        {
            _level2D = level2D;
            _actionStream = new ActionStream(level2D);
            _visited = new HashSet<(int, int)>();
            _unvisited = new Queue<(int, int)>();
            _pathToPos = new Dictionary<(int, int), ((int, int), int)>();
        }

        private void ExploreAlgorithm(int i, int j)
        {
            _pathToPos[(i, j)] = ((0, 0), 0);
            EnqueueUnvisited(_level2D, i, j);

            _pathToPos[(i, j)] = ((0, 0), 0);
            (int, int) deepestPointReached = (i, j);
            (int, int) pos;

            while (_unvisited.Count > 0)
            {
                pos = _unvisited.Dequeue();

                if (pos.Item1 > deepestPointReached.Item1)
                {
                    deepestPointReached = pos;
                }

                _visited.Add(pos);
                EnqueueUnvisited(_level2D, pos.Item1, pos.Item2);
            }

            // Get deepest point and traceback path
            List<(int, int)> path = new List<(int, int)>();
            pos = deepestPointReached;
            while (_pathToPos[pos].Item2 > 0)
            {
                path.Add(_pathToPos[pos].Item1);
                pos = _pathToPos[pos].Item1;
            }

            path.Add((i, j));
            path.Reverse(); // from deepest point --> start to start --> deepest point

            _actionStream.CreateFromPositions(path);
        }

        public async void Explore(int i, int j, System.Action callback)
        {
            await ExploreTask(i, j);
            callback();
        }
        
        private Task ExploreTask(int i, int j)
        {
            ExploreAlgorithm(i, j);
            return Task.CompletedTask;
        }
        

        public ActionStream GetActions()
        {
            return _actionStream;
        }

        private void EnqueueUnvisited(Level2D l2d, int i, int j)
        {
            int currHeight = l2d.Get(i, j);
            if (i + 1 < l2d.Height() && !_visited.Contains((i + 1, j)) &&
                IsNotTooHigh(currHeight, l2d.Get(i + 1, j))) // forward
            {
                _unvisited.Enqueue((i + 1, j));
                AddPath((i + 1, j), (i, j));
            }

            if (j + 1 < l2d.Width() && !_visited.Contains((i, j + 1)) &&
                IsNotTooHigh(currHeight, l2d.Get(i, j + 1))) // right
            {
                _unvisited.Enqueue((i, j + 1));
                AddPath((i, j + 1), (i, j));
            }

            if (i - 1 >= 0 && !_visited.Contains((i - 1, j)) &&
                IsNotTooHigh(currHeight, l2d.Get(i - 1, j))) // left
            {
                _unvisited.Enqueue((i - 1, j));
                AddPath((i - 1, j), (i, j));
            }

            if (j - 1 >= 0 && !_visited.Contains((i, j - 1)) &&
                IsNotTooHigh(currHeight, l2d.Get(i, j - 1))) // back
            {
                _unvisited.Enqueue((i, j - 1));
                AddPath((i, j - 1), (i, j));
            }
        }

        private void AddPath((int, int) target, (int, int) origin)
        {
            int length = _pathToPos[origin].Item2 + 1;
            // if we do not know how to get here before or the path we discovered is shorter
            if (!_pathToPos.ContainsKey(target) || _pathToPos[target].Item2 > length)
            {
                _pathToPos[target] = (origin, length);
            }
        }

        private bool IsNotTooHigh(int currHeight, int targetHeight)
        {
            return currHeight >= targetHeight || targetHeight - currHeight < 2;
        }
    }
}