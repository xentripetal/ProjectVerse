using UnityEngine;
using UnityEngine.Serialization;
using Verse.API;
using Verse.API.Models;
using Verse.Utilities;

namespace Verse.Systems.Visual {
    //TODO Remove as monobehaviour into a scriptedentity controller
    public class PlayerController : MonoBehaviour {
        public static PlayerController Instance;
        private ApiController _apiController;

        private PlayerActual _player;

        private Rigidbody2D _rigidbody2D;

        [FormerlySerializedAs("animator")] [SerializeField]
        public Animator Animator;

        public string HorizontalAxis = "Horizontal";
        [FormerlySerializedAs("runSpeed")] public float RunSpeed = 4f;
        public KeyCode SpeedModifierKey = KeyCode.LeftShift;
        public string VerticalAxis = "Vertical";
        [FormerlySerializedAs("walkSpeed")] public float WalkSpeed = 2.2f;

        private void Awake() {
            Instance = this;
            MappedInput.AddMapping("Up", KeyCode.W);
            MappedInput.AddMapping("Left", KeyCode.A);
            MappedInput.AddMapping("Down", KeyCode.S);
            MappedInput.AddMapping("Right", KeyCode.D);
            MappedInput.AddMapping("Speed Modifier", KeyCode.LeftShift);
            var position = transform.position;
            _player = new PlayerActual();
            _player.SetPosition(new Position(position.x, position.y));
            _player.OnPlayerTeleported += OnPlayerTeleported;
        }

        private void Start() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _apiController = ApiController.Instance;
        }

        private void Update() {
            UpdateAnimator();
            var pos = transform.position;
            _player.SetPosition(new Position(pos.x, pos.y));
            UpdatePlayerSortingPosition(pos.y);
        }

        private void OnPlayerTeleported(Position newPos) {
            _rigidbody2D.position = new Vector2(newPos.x, newPos.y);
        }

        private void FixedUpdate() {
            if (_player.PositionDelta != Position.Zero) {
                var pos = _player.PositionDelta + _player.Position;
                var vectorizedPos = new Vector2(pos.x, pos.y);
                _rigidbody2D.MovePosition(vectorizedPos);
                _player.PositionDelta = Position.Zero;
            }
        }

        private void UpdateAnimator() {
            var currentX = Animator.GetFloat("X");
            var currentY = Animator.GetFloat("Y");

            if (!_player.IsMoving && (!Mathf.Approximately(currentX, 0) || !Mathf.Approximately(currentY, 0))) {
                Animator.SetFloat("lastX", currentX);
                Animator.SetFloat("lastY", currentY);
            }

            Animator.SetBool("isMoving", _player.IsMoving);
            Animator.SetBool("isRunning", _player.IsRunning);
            Animator.SetFloat("X", _player.CurrentInputAxis.x);
            Animator.SetFloat("Y", _player.CurrentInputAxis.y);
        }

        private void UpdatePlayerSortingPosition(float yPosition) {
            var position = transform.position;
            position.z = Constants.ZPositionMultiplier * yPosition + Constants.ZPositionOffset;
            transform.position = position;
        }


        #region Triggers

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("TransparencyCollider")) {
                var sr = other.GetComponentInParent<SpriteRenderer>();
                sr.color = ToggleOpacity(sr.color);
            }
            else {
                _apiController.OnPlayerEnter(other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag("TransparencyCollider")) {
                var sr = other.GetComponentInParent<SpriteRenderer>();
                sr.color = ToggleOpacity(sr.color);
            }
        }

        private Color ToggleOpacity(Color color) {
            if (Mathf.Approximately(color.a, 1.0000f))
                color.a = .8f;
            else
                color.a = 1;

            return color;
        }

        #endregion
    }
}