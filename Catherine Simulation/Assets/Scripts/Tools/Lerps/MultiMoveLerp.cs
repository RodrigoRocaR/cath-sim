using UnityEngine;

namespace Tools.Lerps
{
    public class MultiMoveLerp
    {
        private MoveLerp[] _moveLerps;
        private int _currentIndex;

        public MultiMoveLerp(float[] durations, Vector3[] points)
        {
            if (durations.Length != points.Length-1 && durations.Length > 0)
            {
                Debug.LogError("Wrong initialization of multi move lerp");
            }
            
            _moveLerps = new MoveLerp[points.Length - 1];
            for (int i = 0; i < points.Length - 1; i++)
            {
                _moveLerps[i] = new MoveLerp(durations[i]);
                _moveLerps[i].Setup(points[i], points[i + 1]);
            }

            _currentIndex = 0;
        }

        public Vector3 Lerp()
        {
            if (_moveLerps[_currentIndex].IsCompleted())
            {
                if (_currentIndex < _moveLerps.Length - 1)
                {
                    _currentIndex++;
                }
            }

            return _moveLerps[_currentIndex].Lerp();
        }

        public void Reset()
        {
            foreach (var moveLerp in _moveLerps)
            {
                moveLerp.Reset();
            }

            _currentIndex = 0;
        }

        public Vector3 GetStart()
        {
            return _moveLerps[_currentIndex].GetStart();
        }

        public Vector3 GetEnd()
        {
            return _moveLerps[_currentIndex].GetEnd();
        }

        public bool IsCompleted()
        {
            return _moveLerps[^1].IsCompleted(); // last lerp is completed
        }
    }
}