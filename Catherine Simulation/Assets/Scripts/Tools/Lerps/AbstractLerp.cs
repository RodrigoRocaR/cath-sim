using UnityEngine;

namespace Tools.Lerps
{
    public abstract class AbstractLerp<T>
    {
        protected T Start;
        protected T End;

        private float _elapsedTime;
        protected float Progress;
        protected float Duration;

        protected AbstractLerp(float duration)
        {
            Duration = duration;
        }

        public T Lerp()
        {
            Progress = _elapsedTime / Duration;
            _elapsedTime += Time.deltaTime;
            return Interpolate();
        }

        public abstract T Interpolate();


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
            return Duration;
        }

        public void SetDuration(float duration)
        {
            Duration = duration;
        }
    }
}