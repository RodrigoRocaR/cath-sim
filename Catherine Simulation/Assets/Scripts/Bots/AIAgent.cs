using System.Collections;
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

        private void Start()
        {
            _inputs = Inputs.GetInstance();
            _actionExecutor = new ActionExecutor(_inputs);
            _level2D = new Level2D(Level.GetLevelAsMatrixInt());
        }

        private void Update()
        {
            if (Level.GetPlayerIdentity() != PlayerIdentity.AI) return;
            
            // Manage states
            
        }

        private void PathFinding()
        {
            var bfs = new BFS();
            Vector3 pos = transform.position;
            bfs.Explore(_level2D, (int)pos.z, (int)pos.x);
        }
    }
}