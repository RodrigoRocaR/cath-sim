using UnityEngine;

namespace Tools
{
    public class MoveLerpParabolic : MoveLerp
    {
        private float _yOffset;
        public float Height { get; set; }

        public MoveLerpParabolic(float duration, float height) : base(duration)
        {
            Height = height;
        }

        public override Vector3 Lerp()
        {
            Progress = ElapsedTime / Duration;
            ElapsedTime += Time.deltaTime;
            _yOffset = Height * Mathf.Sin(Progress * Mathf.PI);
            return Vector3.Lerp(StartPos, TargetPos, Progress) + _yOffset * Vector3.up;;
        }
    }
}
