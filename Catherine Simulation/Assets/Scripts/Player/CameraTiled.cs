using System;
using Tools;
using UnityEngine;

namespace Player
{
    public class CameraTiled
    {
        private GameObject _cam;
        private GameObject _target;

        public CameraTiled(GameObject cam, GameObject target)
        {
            _cam = cam;
            _target = target;
        }


        private enum CameraDir
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

        public void LateUpdate()
        {
            _cam.transform.position = _target.transform.position + _offset[(int)_cameraDir];
        }
    
        public void RotateCamera(Vector3 newCamDirVector)
        {
            CameraDir newCamDir = Vector3ToEnum(newCamDirVector);
            int rotation = GetCurrentRotation();
            Vector3 camWorldPivot = new Vector3(0, _cam.transform.position.y, 0); // pivot point in world space
            
            switch (newCamDir)
            {
                case CameraDir.Forward:
                    _cam.transform.RotateAround(camWorldPivot, Vector3.up, RotateHelper.RotateToFront(rotation));
                    break;
                case CameraDir.Backward:
                    _cam.transform.RotateAround(camWorldPivot, Vector3.up, RotateHelper.RotateToBack(rotation));
                    break;
                case CameraDir.Right:
                    _cam.transform.RotateAround(camWorldPivot, Vector3.up, RotateHelper.RotateToRight(rotation));
                    break;
                case CameraDir.Left:
                    _cam.transform.RotateAround(camWorldPivot, Vector3.up, RotateHelper.RotateToLeft(rotation));
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

        private CameraDir Vector3ToEnum(Vector3 dir)
        {
            if (dir == Vector3.forward)
            {
                return CameraDir.Forward;
            }
            if (dir == Vector3.back)
            {
                return CameraDir.Backward;
            }
            if (dir == Vector3.right)
            {
                return CameraDir.Right;
            }
            return CameraDir.Left;
        }

        public void ResetCameraRotation()
        {
            RotateCamera(Vector3.forward);
        }
    }
}
