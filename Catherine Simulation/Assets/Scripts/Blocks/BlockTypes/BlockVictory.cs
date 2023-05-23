using LevelDS;
using Player;
using UnityEngine;
using TMPro;

namespace Blocks.BlockTypes
{
    public class BlockVictory : IBlock
    {
        private GameObject _victoryCanvas;

        public BlockVictory(GameObject victoryCanvas)
        {
            _victoryCanvas = victoryCanvas;
        }

        public void OnPlayerStepOn()
        {
            if (Level.IsCleared()) return;
            // Game victory
            _victoryCanvas.SetActive(true);
            setTimerOnVictoryCanvas();
            Level.Finish();
        }

        // Can not be moved
        public void TriggerPull(Transform playerTransform, PlayerState playerState, bool goingToHang = false)
        {
        }

        public void TriggerPush(Transform playerTransform, PlayerState playerState)
        {
        }

        private void setTimerOnVictoryCanvas()
        {
            TextMeshProUGUI[] timerTextVictoryArray = _victoryCanvas.GetComponentsInChildren<TextMeshProUGUI>();
            TextMeshProUGUI timerTextVictory = null;
            foreach (var textMeshPro in timerTextVictoryArray)
            {
                if (textMeshPro.name == GameConstants.TimerTextName)
                {
                    timerTextVictory = textMeshPro;
                }
            }
            
            GameObject timerTextCornerGameObject = GameObject.Find(GameConstants.TimerDisplayOnCornerName);
            if (timerTextCornerGameObject == null)
            {
                Debug.LogWarning("Could not find timer display game object");
                return;
            }
            TextMeshProUGUI timerTextCorner = timerTextCornerGameObject.GetComponentInChildren<TextMeshProUGUI>();

            if (timerTextVictory != null && timerTextCorner != null)
            {
                timerTextVictory.SetText(timerTextCorner.text);
            }
            else
            {
                if (timerTextVictory == null) Debug.LogWarning("Could not find timer text on victory canvas");
                if (timerTextCorner == null) Debug.LogWarning("Could not find timer text on corner display");
            }
        }
    }
}