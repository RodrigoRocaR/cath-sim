using UnityEngine;

namespace Tools.Lerps
{
    public class RotateSlerp : AbstractLerp<Quaternion>
    {

        public RotateSlerp(float duration) : base(duration)
        {
        }

        protected override Quaternion Interpolate()
        {
            return Quaternion.Slerp(Start, End, Progress);
        }
    }
}