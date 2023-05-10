using System;
using Tools;
using Tools.Lerps;
using UnityEngine;

namespace Player
{
    public class CameraTiled
    {

        private GameObject _cam;
        private GameObject _target;

        private bool _rotating;
        private RotateSlerp _rotateSlerp;
        private MoveLerp _moveLerp;
        
        private Vector3 _targetFinalPos;
        private float _duration;

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
            if (_rotating)
            {
                if (!_moveLerp.HasStarted())
                {
                    _moveLerp.Setup(_cam.transform.position, _targetFinalPos + _offset[(int)_cameraDir]);
                }

                if (!_rotateSlerp.IsCompleted()) _cam.transform.rotation = _rotateSlerp.Lerp();
                if (!_moveLerp.IsCompleted()) _cam.transform.position = _moveLerp.Lerp();

                if (_rotateSlerp.IsCompleted() && _moveLerp.IsCompleted())
                {
                    _rotateSlerp.Reset();
                    _moveLerp.Reset();
                    _rotating = false;
                }
            }
            else
            {
                _cam.transform.position = _target.transform.position + _offset[(int)_cameraDir];
            }
        }

        public void RotateCameraSmooth(Vector3 newCamDirVector, Vector3 targetFinalPos, float duration)
        {
            _targetFinalPos = targetFinalPos;
            CameraDir newCamDir = Vector3ToEnum(newCamDirVector);
            
            if (_cameraDir == newCamDir) return;
            _cameraDir = newCamDir;

            if (duration < 0f)
            {
                _rotateSlerp = null;
                _moveLerp = null;
                return;
            }
            
            
            _rotateSlerp = new RotateSlerp(duration);
            _moveLerp = new MoveLerp(duration);
            _rotating = true;
            

            Vector3 globalRotation = _cam.transform.rotation.eulerAngles;
            float camRotX = globalRotation.x, camRotZ = globalRotation.z;

            switch (newCamDir)
            {
                case CameraDir.Forward:
                    _rotateSlerp.Setup(globalRotation, new Vector3(camRotX, 0, camRotZ));
                    break;
                case CameraDir.Backward:
                    _rotateSlerp.Setup(globalRotation, new Vector3(camRotX, 180, camRotZ));
                    break;
                case CameraDir.Right:
                    _rotateSlerp.Setup(globalRotation, new Vector3(camRotX, 90, camRotZ));
                    break;
                case CameraDir.Left:
                    _rotateSlerp.Setup(globalRotation, new Vector3(camRotX, -90, camRotZ));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void RotateCamera(Vector3 newCamDirVector)
        {
            CameraDir newCamDir = Vector3ToEnum(newCamDirVector);
            if (_cameraDir == newCamDir) return;
            
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
            _rotating = false;
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