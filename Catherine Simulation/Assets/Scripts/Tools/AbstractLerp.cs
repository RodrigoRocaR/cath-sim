using UnityEngine;

namespace Tools
{
    public abstract class AbstractLerp<T>
    {
        private T _start;
        private T _end;

        private float _elapsedTime;
        private float _progress;
        private float _duration;

        protected AbstractLerp(float duration)
        {
            _duration = duration;
        }

        public virtual T Lerp()
        {
            _progress = _elapsedTime / _duration;
            _elapsedTime += Time.deltaTime;
            return InterpolateWithProgress();
        }

        protected abstract T InterpolateWithProgress();


        public void Setup(T start, T end)
        {
            _start = start;
            _end = end;
        }

        public bool IsCompleted()
        {
            return _progress >= 1f;
        }

        public void Reset()
        {
            _progress = 0f;
            _elapsedTime = 0f;
        }

        public T GetStart()
        {
            return _start;
        }
        
        public T GetEnd()
        {
            return _end;
        }
    }
}