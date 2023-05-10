using UnityEngine;

namespace Tools.Lerps
{
    public abstract class AbstractLerp<T>
    {
        protected T Start;
        protected T End;

        private float _elapsedTime;
        protected float Progress;
        private float _duration;

        protected AbstractLerp(float duration)
        {
            _duration = duration;
        }

        public T Lerp()
        {
            Progress = _elapsedTime / _duration;
            _elapsedTime += Time.deltaTime;
            return Interpolate();
        }

        protected abstract T Interpolate();


        public void Setup(T start, T end)
        {
            Start = start;
            End = end;
        }

        public bool IsCompleted()
        {
            return Progress >= 1f;
        }

        public void Reset()
        {
            Progress = 0f;
            _elapsedTime = 0f;
        }

        public T GetStart()
        {
            return Start;
        }
        
        public T GetEnd()
        {
            return End;
        }

        public float GetDuration()
        {
            return _duration;
        }

        public void SetDuration(float duration)
        {
            _duration = duration;
        }
    }
}