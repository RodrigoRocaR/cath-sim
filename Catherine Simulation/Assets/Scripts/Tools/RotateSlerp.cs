using UnityEngine;

namespace Tools
{
    public class RotateSlerp
    {
        protected Quaternion TargetPos;
        protected Quaternion StartPos;
        protected float Progress;
        protected float ElapsedTime; // in seconds
        public float Duration { get; set; }  // total duration in seconds

        public RotateSlerp(float duration)
        {
            Duration = duration;
        }
    
        public virtual Quaternion Lerp()
        {
            Progress = ElapsedTime / Duration;
            ElapsedTime += Time.deltaTime;
            return Quaternion.Slerp(StartPos, TargetPos, Progress);
        }

        public void Setup(Vector3 startRotation, Vector3 endRotation)
        {
            StartPos = Quaternion.Euler(startRotation);
            TargetPos = Quaternion.Euler(endRotation);
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

        public Quaternion GetStart()
        {
            return StartPos;
        }
        
        public Quaternion GetEnd()
        {
            return TargetPos;
        }
        
    }
}