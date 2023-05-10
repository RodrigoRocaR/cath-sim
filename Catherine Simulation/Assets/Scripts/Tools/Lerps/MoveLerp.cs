using UnityEngine;

namespace Tools.Lerps
{
    public class MoveLerp : AbstractLerp<Vector3>
    {
        public MoveLerp(float duration) : base(duration)
        {
        }

        public override Vector3 Interpolate()
        {
            return Vector3.Lerp(Start, End, Progress);
        }
    }
}