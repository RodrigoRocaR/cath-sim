using LevelDS;
using UnityEngine;

namespace Player.Controllers
{
    public class GameOverController
    {
        private readonly GameObject _gameOverCanvas;
        private readonly Transform _playerTransform;

        public GameOverController (Transform playerTransform, GameObject gameOverCanvas)
        {
            _playerTransform = playerTransform;
            _gameOverCanvas = gameOverCanvas;
        }

        public void CheckForGameOver()
        {
            if (_playerTransform.position.y > GameConstants.GameOverMinimumY) return;
            
            Level.GameOver();
            GameObject gameOverCanvasInstance = Object.Instantiate(_gameOverCanvas);
        }
    }
}