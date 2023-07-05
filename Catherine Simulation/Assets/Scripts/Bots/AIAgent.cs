using Bots.Action;
using Bots.Algorithms;
using Bots.DS;
using LevelDS;
using Player;
using UnityEngine;

namespace Bots
{
    public class AIAgent : MonoBehaviour
    {
        private Inputs _inputs;
        private ActionExecutor _actionExecutor;
        private Level2D _level2D;
        private BotState _botState;
        private Rigidbody _rb;
        
        private BFS _bfs;
        private MonteCarlo _mcts;

        private void Start()
        {
            _inputs = Inputs.GetInstance();
            _actionExecutor = new ActionExecutor(_inputs);
            _level2D = new FloorLevel2D(Level.GetLevelAsMatrixInt());
            _botState = new BotState();
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (Level.GetPlayerIdentity() != PlayerIdentity.AI) return;
            if (IsFalling()) return;

            if (_botState.CanExplore())
            {
                Explore();
            } 
            else if (_botState.CanClimb())
            {
                Climb();
            }
        }

        private void Explore()
        {
            _bfs = new BFS(_level2D);
            Vector3 pos = transform.position;
            _botState.StartExploring();
            _bfs.Explore((int)pos.x, (int)pos.z);
            
            BotEventManager.OnExplorationFinished += OnFinishExplore;
            LookForClimbingRoutes();
            StartCoroutine(_actionExecutor.Execute(_bfs.GetActions(), ActionExecutorPurpose.Exploration));
        }

        private void Climb()
        {
            var actions = _mcts.GetActions();
            // todo: execute them
        }

        private void OnFinishExplore()
        {
            _bfs = null;
            _botState.StopExploring();
        }

        private void LookForClimbingRoutes()
        {
            _mcts = new MonteCarlo(_bfs.GetEndPlayerPos());
            StartCoroutine(_mcts.LookForClimbingRoutes());
        }

        private bool IsFalling()
        {
            return !_rb.IsSleeping() && _rb.velocity.y < -0.1;
        }
    }
}