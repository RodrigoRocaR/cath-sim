using System.Threading;
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
            _level2D = new FloorLevel2D(Level.GetLevelAsMatrixInt()); // level changed after climbing or not ini
            _bfs = new BFS(_level2D);
            Vector3 pos = transform.position;
            _botState.StartExploring();
            var endPos = _bfs.GetUpIfHanging(pos);
            _bfs.Explore((int)endPos.x, (int)endPos.z);
            
            BotEventManager.OnExplorationFinished += OnFinishExplore;
            LookForClimbingRoutes();
            StartCoroutine(_actionExecutor.Execute(_bfs.GetActions(), ActionExecutorPurpose.Exploration));
        }

        private void Climb()
        {
            ActionStream actionStream = new ActionStream(_level2D);
            actionStream.CreateFromPushPullActions(_bfs.GetEndPlayerPos(), _mcts.GetActions());
            _botState.StartClimbing();
            BotEventManager.OnClimbFinished += OnFinishClimb;
            StartCoroutine(_actionExecutor.Execute(actionStream, ActionExecutorPurpose.Climbing));
        }

        private void OnFinishExplore()
        {
            _botState.StopExploring();
        }

        private void OnFinishClimb()
        {
            _botState.StopClimbing();
        }

        private void LookForClimbingRoutes()
        {
            _mcts = new MonteCarlo(_bfs.GetEndPlayerPos());
            Thread t = new Thread(_mcts.LookForClimbingRoutes);
            t.Start();
        }

        private bool IsFalling()
        {
            return !_rb.IsSleeping() && _rb.velocity.y < -0.1;
        }
    }
}