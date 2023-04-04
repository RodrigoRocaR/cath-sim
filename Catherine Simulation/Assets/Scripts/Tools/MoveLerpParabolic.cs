using UnityEngine;

namespace Tools
{
    public class MoveLerpParabolic : MoveLerp
    {
        public float YOffset { get; set; }
        public float Height { get; set; }

        public MoveLerpParabolic(float duration, float height) : base(duration)
        {
            Height = height;
        }

        public override Vector3 Lerp()
        {
            Progress = ElapsedTime / Duration;
            ElapsedTime += Time.deltaTime;
            YOffset = Height * Mathf.Sin(Progress * Mathf.PI);
            return Vector3.Lerp(StartPos, TargetPos, Progress) + YOffset * Vector3.up;;
        }
    }
}
