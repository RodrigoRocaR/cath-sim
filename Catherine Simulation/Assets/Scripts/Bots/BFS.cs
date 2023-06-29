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

        private List<(int, int)> _path;

        private bool _isMock;

        public BFS(Level2D level2D, bool isMock = false)
        {
            _level2D = level2D;
            _isMock = isMock;

            _actionStream = new ActionStream(level2D);
            _visited = new HashSet<(int, int)>();
            _unvisited = new Queue<(int, int)>();
            _pathToPos = new Dictionary<(int, int), ((int, int), int)>();
        }

        public void Explore(int i, int j)
        {
            i /= GameConstants.BlockScale;
            j /= GameConstants.BlockScale;
            
            _pathToPos[(i, j)] = ((0, 0), 0);
            EnqueueUnvisited(i, j);

            (int, int) deepestPointReached = (i, j);
            (int, int) pos;

            while (_unvisited.Count > 0)
            {
                pos = _unvisited.Dequeue();

                if (pos.Item2 > deepestPointReached.Item2)
                {
                    deepestPointReached = pos;
                }
                
                EnqueueUnvisited(pos.Item1, pos.Item2);
            }

            // Get deepest point and traceback path
            _path = new List<(int, int)> { deepestPointReached };
            pos = deepestPointReached;
            while (_pathToPos[pos].Item2 > 0)
            {
                _path.Add(_pathToPos[pos].Item1);
                pos = _pathToPos[pos].Item1;
            }
            
            _path.Reverse(); // from deepest point --> start to start --> deepest point
            
            _actionStream.CreateFromPositions(_path);
        }

        public ActionStream GetActions()
        {
            return _actionStream;
        }

        public List<(int, int)> GetPath()
        {
            return _path;
        }

        private void EnqueueUnvisited(int i, int j)
        {
            _visited.Add((i, j));
            int currHeight = _level2D.Get(i, j);
            int nextHeight = i + 1 < _level2D.Height() ? _level2D.Get(i + 1, j) : GameConstants.EmptyBlock;
            if (nextHeight != GameConstants.EmptyBlock && !_visited.Contains((i + 1, j)) &&
                IsNotTooHigh(currHeight, nextHeight)) // forward
            {
                _unvisited.Enqueue((i + 1, j));
                AddPath((i + 1, j), (i, j));
                _visited.Add((i + 1, j));
            }

            nextHeight = j + 1 < _level2D.Width() ? _level2D.Get(i, j + 1) : GameConstants.EmptyBlock;
            if (nextHeight != GameConstants.EmptyBlock && !_visited.Contains((i, j + 1)) &&
                IsNotTooHigh(currHeight, nextHeight)) // right
            {
                _unvisited.Enqueue((i, j + 1));
                AddPath((i, j + 1), (i, j));
                _visited.Add((i, j + 1));
            }

            nextHeight = i - 1 >= 0 ? _level2D.Get(i - 1, j) : GameConstants.EmptyBlock;
            if (nextHeight != GameConstants.EmptyBlock && !_visited.Contains((i - 1, j)) &&
                IsNotTooHigh(currHeight, nextHeight)) // left
            {
                _unvisited.Enqueue((i - 1, j));
                AddPath((i - 1, j), (i, j));
                _visited.Add((i - 1, j));
            }

            nextHeight = j - 1 >= 0 ? _level2D.Get(i, j - 1) : GameConstants.EmptyBlock;
            if (nextHeight != GameConstants.EmptyBlock && !_visited.Contains((i, j - 1)) &&
                IsNotTooHigh(currHeight, nextHeight)) // back
            {
                _unvisited.Enqueue((i, j - 1));
                AddPath((i, j - 1), (i, j));
                _visited.Add((i, j - 1));
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

        public Queue<(int, int)> GetUnvisited()
        {
            return _isMock ? _unvisited : null;
        }

        public HashSet<(int, int)> GetVisited()
        {
            return _isMock ? _visited : null;
        }

        public Dictionary<(int, int), ((int, int), int)> GetAllPaths()
        {
            return _isMock ? _pathToPos : null;
        }

        public void SetPathToPos(Dictionary<(int, int), ((int, int), int)> p)
        {
            if (!_isMock) return;
            _pathToPos = p;
        }

        public void SetVisited(HashSet<(int, int)> visited)
        {
            if (!_isMock) return;
            _visited = visited;
        }
    }
}