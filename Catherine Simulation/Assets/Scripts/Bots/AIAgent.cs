﻿using Bots.Action;
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
        private BFS _bfs;

        private void Start()
        {
            _inputs = Inputs.GetInstance();
            _actionExecutor = new ActionExecutor(_inputs);
            _level2D = new Level2D(Level.GetLevelAsMatrixInt());
            _botState = new BotState();
        }

        private void Update()
        {
            if (Level.GetPlayerIdentity() != PlayerIdentity.AI) return;

            if (_botState.CanExplore())
            {
                Explore();
            } 
            else
            {
                // climb
            }

        }

        private void Explore()
        {
            _bfs = new BFS(_level2D);
            Vector3 pos = transform.position;
            _botState.StartExploring();
            _bfs.Explore((int)pos.z, (int)pos.x, OnFinishExplore);
        }

        private void OnFinishExplore()
        {
            _actionExecutor.Execute(_bfs.GetActions());
            _bfs = null;
            _botState.StopExploring();
        }
    }
}