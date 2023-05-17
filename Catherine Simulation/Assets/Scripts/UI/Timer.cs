using LevelDS;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Timer : MonoBehaviour
    {
        public Text timerText;
        private float _startTime;

        private void Start()
        {
            _startTime = Time.time;
        }

        private void Update()
        {
            if (Level.IsCleared()) return;
            float elapsedTime = Time.time - _startTime;

            int minutes = (int)(elapsedTime / 60);
            int seconds = (int)(elapsedTime % 60);
            int milliseconds = (int)(elapsedTime * 1000 % 1000);

            string timerString = $"{minutes:00}:{seconds:00}.{milliseconds:000}";
            timerText.text = timerString;
        }
    }
}