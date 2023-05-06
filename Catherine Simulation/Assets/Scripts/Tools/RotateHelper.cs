using UnityEngine;

namespace Tools
{
    public static class RotateHelper
    {
        public static int GetCurrentRotation(Vector3 eulerAngles)
        {
            float rotation = eulerAngles.y;
            return rotation switch
            {
                < 272 and > 268 => 270,
                < 182 and > 178 => 180,
                < 92 and > 88 => 90,
                _ => 0
            };
        }

        public static int RotateToLeft(Vector3 eulerAngles)
        {
            return RotateToLeft(GetCurrentRotation(eulerAngles));
        }

        public static int RotateToLeft(int currentRotation)
        {
            return currentRotation switch
            {
                0 => -90,
                180 => 90,
                270 => 0,
                _ => 180
            };
        }
        
        public static int RotateToRight(Vector3 eulerAngles)
        {
            return RotateToRight(GetCurrentRotation(eulerAngles));
        }

        public static int RotateToRight(int currentRotation)
        {
            return currentRotation switch
            {
                0 => 90,
                90 => 0,
                180 => -90,
                _ => -180
            };
        }
        
        public static int RotateToBack(Vector3 eulerAngles)
        {
            return RotateToBack(GetCurrentRotation(eulerAngles));
        }

        public static int RotateToBack(int currentRotation)
        {
            return currentRotation switch
            {
                0 => 180,
                180 => 0,
                _ => currentRotation
            };
        }
        
        public static int RotateToFront(Vector3 eulerAngles)
        {
            return RotateToLeft(GetCurrentRotation(eulerAngles));
        }

        public static int RotateToFront(int currentRotation)
        {
            return currentRotation switch
            {
                >1 or <-1 => -currentRotation,
                _ => 0
            };
        }
    }
}