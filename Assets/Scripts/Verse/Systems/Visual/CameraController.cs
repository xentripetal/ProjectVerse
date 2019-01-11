using UnityEngine;
using Verse.API;
using Verse.API.Models;

namespace Verse.Systems.Visual {
    public class CameraController : MonoBehaviour {
        public Transform Target;
        public float SmoothSpeed = .125f;
        public Vector3 Offset;
        private Vector3 _smoothedPosition;
        private RoomController _roomController;
        private Camera _camera;

        private void Start() {
            _roomController = RoomController.Instance;
            _camera = Camera.main;
        }


        void FixedUpdate() {
            Vector3 desiredPosition = Target.position + Offset;
            _smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed);

            float camVertExtent = _camera.orthographicSize;
            float camHorzExtent = _camera.aspect * camVertExtent;

            Position topRightBound = _roomController.TopRight;
            Position bottomLeftBound = _roomController.BottomLeft;

            float leftBound = bottomLeftBound.x + camHorzExtent;
            float rightBound = topRightBound.x - camHorzExtent;
            float bottomBound = bottomLeftBound.y + camVertExtent;
            float topBound = topRightBound.y - camVertExtent + Player.Main.Height;

            if (topBound < bottomBound) {
                topBound = _roomController.Center.y;
                bottomBound = topBound;
            }

            if (rightBound < leftBound) {
                rightBound = _roomController.Center.x;
                leftBound = rightBound;
            }

            _smoothedPosition = new Vector3(
                Mathf.Clamp(_smoothedPosition.x, leftBound, rightBound),
                Mathf.Clamp(_smoothedPosition.y, bottomBound, topBound),
                _smoothedPosition.z
            );

            transform.position = _smoothedPosition;
        }
    }
}