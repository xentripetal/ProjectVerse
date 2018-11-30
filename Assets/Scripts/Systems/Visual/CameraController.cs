using UnityEngine;

namespace Verse.Systems.Visual {
    public class CameraController : MonoBehaviour {
        public Transform target;
        public float smoothSpeed = .125f;
        public Vector3 offset;
        private Vector3 smoothedPosition;

        void FixedUpdate() {
            Vector3 desiredPosition = target.position + offset;
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}