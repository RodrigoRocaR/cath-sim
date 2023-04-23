using UnityEngine;

namespace Tools
{
    public class MoveLerp
    {
        protected Vector3 TargetPos;
        protected Vector3 StartPos;
        protected float Progress;
        protected float ElapsedTime; // in seconds
        public float Duration { get; set; }  // total duration in seconds

        public MoveLerp(float duration)
        {
            Duration = duration;
        }
    
        public virtual Vector3 Lerp()
        {
            Progress = ElapsedTime / Duration;
            ElapsedTime += Time.deltaTime;
            return Vector3.Lerp(StartPos, TargetPos, Progress);
        }

        public void Setup(Vector3 start, Vector3 end)
        {
            StartPos = start;
            TargetPos = end;
        }

        public bool IsCompleted()
        {
            return Progress >= 1f;
        }

        public void Reset()
        {
            Progress = 0f;
            ElapsedTime = 0f;
        }

        public Vector3 GetStart()
        {
            return StartPos;
        }
        
        public Vector3 GetEnd()
        {
            return TargetPos;
        }
    }
}
