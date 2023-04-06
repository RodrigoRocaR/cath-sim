using UnityEngine;

namespace Tools
{
    public class MoveLerp
    {
        public Vector3 TargetPos { get; set; }
        public Vector3 StartPos { get; set; }
        public float Progress { get; set; }
        public float ElapsedTime { get; set; }  // in seconds
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

        public bool IsCompleted()
        {
            return Progress >= 1f;
        }

        public void Reset()
        {
            Progress = 0f;
            ElapsedTime = 0f;
        }
    }
}
