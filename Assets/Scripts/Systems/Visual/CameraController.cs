using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Verse.API.Models;

namespace Verse.Systems.Visual {
    public class CameraController : MonoBehaviour {
        public Transform target;
        public float smoothSpeed = .125f;
        public Vector3 offset;
        private Vector3 smoothedPosition;
        private RoomController _roomController;

        private void Start() {
            _roomController = RoomController.Instance;
        }


        void FixedUpdate() {
            var cam = Camera.main;
            
            Vector3 desiredPosition = target.position + offset;
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            
            float camVertExtent = cam.orthographicSize;
            float camHorzExtent = cam.aspect * camVertExtent;
            
            Position topRightBound = _roomController.TopRight;
            Position bottomLeftBound = _roomController.BottomLeft;
            
            float leftBound = bottomLeftBound.x + camHorzExtent;
            float rightBound = topRightBound.x - camHorzExtent;
            float bottomBound = bottomLeftBound.y + camVertExtent;
            float topBound = topRightBound.y - camVertExtent + Player.Height;

            if (topBound < bottomBound) {
                topBound = _roomController.Center.y;
                bottomBound = topBound;
            }

            if (rightBound < leftBound) {
                rightBound = _roomController.Center.x;
                leftBound = rightBound;
            }
            smoothedPosition = new Vector3(
                Mathf.Clamp(smoothedPosition.x, leftBound, rightBound),
                Mathf.Clamp(smoothedPosition.y, bottomBound, topBound),
                smoothedPosition.z
            );

            transform.position = smoothedPosition;
        }
    }
}