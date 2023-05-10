using UnityEngine;

namespace Tools.Lerps
{
    public class MoveLerpParabolic : MoveLerp
    {
        private float _yOffset;
        private float _height;

        public MoveLerpParabolic(float duration, float height) : base(duration)
        {
            _height = height;
        }

        protected override Vector3 Interpolate()
        {
            _yOffset = _height * Mathf.Sin(Progress * Mathf.PI);
            return Vector3.Lerp(Start, End, Progress) + _yOffset * Vector3.up;;
        }
    }
}
