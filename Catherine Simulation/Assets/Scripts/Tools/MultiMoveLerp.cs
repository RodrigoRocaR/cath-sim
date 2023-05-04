using UnityEngine;

namespace Tools
{
    public class MultiMoveLerp
    {
        protected MoveLerp[] MoveLerps;
        protected int CurrentIndex;

        public MultiMoveLerp(float[] durations, Vector3[] points)
        {
            if (durations.Length != points.Length-1 && durations.Length > 0)
            {
                Debug.LogError("Wrong initialization of multi move lerp");
            }
            
            MoveLerps = new MoveLerp[points.Length - 1];
            for (int i = 0; i < points.Length - 1; i++)
            {
                MoveLerps[i] = new MoveLerp(durations[i]);
                MoveLerps[i].Setup(points[i], points[i + 1]);
            }

            CurrentIndex = 0;
        }

        public Vector3 Lerp()
        {
            if (MoveLerps[CurrentIndex].IsCompleted())
            {
                if (CurrentIndex < MoveLerps.Length - 1)
                {
                    CurrentIndex++;
                }
            }

            return MoveLerps[CurrentIndex].Lerp();
        }

        public void Reset()
        {
            foreach (var moveLerp in MoveLerps)
            {
                moveLerp.Reset();
            }

            CurrentIndex = 0;
        }

        public Vector3 GetStart()
        {
            return MoveLerps[CurrentIndex].GetStart();
        }

        public Vector3 GetEnd()
        {
            return MoveLerps[CurrentIndex].GetEnd();
        }

        public bool IsCompleted()
        {
            return MoveLerps[^1].IsCompleted(); // last lerp is completed
        }
    }
}