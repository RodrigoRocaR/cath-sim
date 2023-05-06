using System;
using Tools;
using UnityEngine;

namespace Player
{
    public class Camera : MonoBehaviour
    {
        public GameObject target;

        public enum CameraDir
        {
            Forward = 0,
            Backward = 1,
            Right = 2,
            Left = 3
        }

        private CameraDir _cameraDir = CameraDir.Forward;
            
        private readonly Vector3[] _offset =
        {
            new Vector3(0, 4, -6.5f), // forward
            new Vector3(0, 4, 6.5f), // backward
            new Vector3(-6.5f, 4, 0), // right
            new Vector3(6.5f, 4, 0) // left
        };

        void LateUpdate()
        {
            transform.position = target.transform.position + _offset[(int)_cameraDir];
        }
    
        public void RotateCamera(CameraDir newCamDir)
        {
            int rotation = GetCurrentRotation();
            switch (newCamDir)
            {
                case CameraDir.Forward:
                    transform.Rotate(new Vector3(0, RotateHelper.RotateToFront(rotation), 0));
                    break;
                case CameraDir.Backward:
                    transform.Rotate(new Vector3(0, RotateHelper.RotateToBack(rotation), 0));
                    break;
                case CameraDir.Right:
                    transform.Rotate(new Vector3(0, RotateHelper.RotateToRight(rotation), 0));
                    break;
                case CameraDir.Left:
                    transform.Rotate(new Vector3(0, RotateHelper.RotateToLeft(rotation), 0));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _cameraDir = newCamDir;
        }

        private int GetCurrentRotation()
        {
            return _cameraDir switch
            {
                CameraDir.Forward => 0,
                CameraDir.Backward => 180,
                CameraDir.Right => 90,
                CameraDir.Left => -90,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
