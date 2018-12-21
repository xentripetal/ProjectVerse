using UnityEngine;
using UnityEngine.Serialization;
using Verse.API;
using Verse.API.Models;
using Verse.Utilities;

namespace Verse.Systems.Visual {
    public class PlayerController : MonoBehaviour {
        [FormerlySerializedAs("walkSpeed")] public float WalkSpeed = 2.2f;
        [FormerlySerializedAs("runSpeed")] public float RunSpeed = 4f;

        [FormerlySerializedAs("animator")] [SerializeField]
        public Animator Animator;

        public static PlayerController Instance;

        public string HorizontalAxis = "Horizontal";
        public string VerticalAxis = "Vertical";
        public KeyCode SpeedModifierKey = KeyCode.LeftShift;

        private Player _playerData;
        private Vector2 _playerInput;
        private bool _isMoving;
        private bool _isRunning;

        private bool _moveToCalled;

        private Rigidbody2D _rigidbody2D;
        private ApiController _apiController;

        void Awake() {
            Instance = this;
            _playerData = new Player();
        }

        private void Start() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _apiController = ApiController.Instance;
            _playerData.OnRequestedPlayerMove += OnRequestedPlayerMove;
            _playerData.OnPlayerMoved += OnPlayerMoved;
        }

        void Update() {
            GetPlayerInput();
            UpdateAnimator();
        }

        void LateUpdate() {
            UpdateModel();
        }

        private void FixedUpdate() {
            HandlePlayerMovement();
        }

        
        #region Triggers
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("TransparencyCollider")) {
                var sr = other.GetComponentInParent<SpriteRenderer>();
                sr.color = ToggleOpacity(sr.color);
            }
            else {
                _apiController.OnPlayerEnter(_playerData, other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag("TransparencyCollider")) {
                var sr = other.GetComponentInParent<SpriteRenderer>();
                sr.color = ToggleOpacity(sr.color);
            }
        }

        private Color ToggleOpacity(Color color) {
            if (Mathf.Approximately(color.a, 1.0000f)) {
                color.a = .8f;
            }
            else {
                color.a = 1;
            }

            return color;
        }

        #endregion
        
        #region ModelAndAnimator

        private void OnPlayerMoved(PlayerPosition newPos, PlayerPosition oldPos) {
            if (!_moveToCalled) {
                Debug.Log("Scripted Teleport");
                transform.position = ApiMappings.Vector2FromPosition(newPos);
                UpdatePlayerSortingPosition(newPos.y);
            }
        }

        private void UpdateModel() {
            var currentPos = new PlayerPosition(transform.position.x, transform.position.y);
            if (_playerData.Position != currentPos) {
                UpdatePlayerSortingPosition(currentPos.y);
                _moveToCalled = true;
                _playerData.MoveToWithoutPhysics(currentPos);
                _moveToCalled = false;
            }
        }

        private void UpdateAnimator() {
            float currentX = Animator.GetFloat("X");
            float currentY = Animator.GetFloat("Y");

            if (!_isMoving && (!Mathf.Approximately(currentX, 0) || !Mathf.Approximately(currentY, 0))) {
                Animator.SetFloat("lastX", currentX);
                Animator.SetFloat("lastY", currentY);
            }

            Animator.SetBool("isMoving", _isMoving);
            Animator.SetBool("isRunning", _isRunning);
            Animator.SetFloat("X", _playerInput.x);
            Animator.SetFloat("Y", _playerInput.y);
        }

        #endregion

        #region Movement Code

        private void OnRequestedPlayerMove(PlayerPosition pos) {
            _rigidbody2D.MovePosition(transform.position + ApiMappings.Vector3FromPosition(pos));
        }
        
        private void GetPlayerInput() {
            _isRunning = !Input.GetKey(SpeedModifierKey);
            _playerInput = new Vector2(Input.GetAxisRaw(HorizontalAxis), Input.GetAxisRaw(VerticalAxis));
            _isMoving = Mathf.Abs(_playerInput.x) + Mathf.Abs(_playerInput.y) > 0;
        }

        private void HandlePlayerMovement() {
            if (_isMoving) {
                MovePlayer(_playerInput);
            }
        }

        private void MovePlayer(Vector2 playerInput) {
            float playerSpeed = GetPlayerSpeed();
            Vector2 movePosition = CalculateMoveVector(playerInput, playerSpeed);
            _playerData.Move(ApiMappings.Vector2ToPosition(movePosition));
        }

        private void UpdatePlayerSortingPosition(float yPosition) {
            Vector3 position = transform.position;
            position.z = Constants.ZPositionMultiplier * yPosition + Constants.ZPositionOffset;
            transform.position = position;
        }

        private float GetPlayerSpeed() {
            return _isRunning ? RunSpeed : WalkSpeed;
        }

        private Vector2 CalculateMoveVector(Vector2 playerInput, float speed) {
            return playerInput.normalized * Time.fixedDeltaTime * speed;
        }

        #endregion
    }
}