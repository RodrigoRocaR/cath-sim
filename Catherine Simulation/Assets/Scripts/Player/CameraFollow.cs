using UnityEngine;

namespace Player
{
    public class CameraFollow : MonoBehaviour
    {
        public GameObject target;
        private readonly Vector3 _offset = new Vector3(0, 4, -6.5f);
        private readonly Vector3 _offsetAngle = new Vector3(0, 0, 0);

        void LateUpdate()
        {
            transform.position = target.transform.position + _offset;
            transform.Rotate(_offsetAngle);
        }
    }
}
