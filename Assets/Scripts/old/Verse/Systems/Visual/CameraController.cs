using UnityEngine;
using Verse.API;

namespace Verse.Systems.Visual {
    public class CameraController : MonoBehaviour {
        private Camera _camera;
        private RoomController _roomController;
        private Vector3 _smoothedPosition;
        public Vector3 Offset;
        public float SmoothSpeed = .125f;
        public Transform Target;

        private void Start() {
            _roomController = RoomController.Instance;
            _camera = Camera.main;
        }


        private void FixedUpdate() {
            var desiredPosition = Target.position + Offset;
            _smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed);

            var camVertExtent = _camera.orthographicSize;
            var camHorzExtent = _camera.aspect * camVertExtent;

            var topRightBound = _roomController.TopRight;
            var bottomLeftBound = _roomController.BottomLeft;

            var leftBound = bottomLeftBound.x + camHorzExtent;
            var rightBound = topRightBound.x - camHorzExtent;
            var bottomBound = bottomLeftBound.y + camVertExtent;
            var topBound = topRightBound.y - camVertExtent + Player.Main.Height;

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