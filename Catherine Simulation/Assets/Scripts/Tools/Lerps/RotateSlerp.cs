using UnityEngine;

namespace Tools.Lerps
{
    public class RotateSlerp : AbstractLerp<Quaternion>
    {

        public RotateSlerp(float duration) : base(duration)
        {
        }

        public override Quaternion Interpolate()
        {
            return Quaternion.Slerp(Start, End, Progress);
        }
    }
}