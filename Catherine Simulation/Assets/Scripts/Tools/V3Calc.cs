using UnityEngine;

namespace Tools
{
    public class V3Calc
    {
        public static Vector3 SubstractNum(Vector3 vector, float scalar)
        {
            return new Vector3(vector.x - scalar, vector.y - scalar, vector.z - scalar);
        }
        
        public static Vector3 AddNum(Vector3 vector, float scalar)
        {
            return new Vector3(vector.x + scalar, vector.y + scalar, vector.z + scalar);
        }
    }
}