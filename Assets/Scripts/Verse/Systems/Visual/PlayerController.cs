using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Verse.API;
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
        private float2 _playerInput;
        private bool _isMoving;
        private bool _isRunning;

        private Rigidbody2D _rigidbody2D;
        private ApiController _apiController;

        void Awake() {
            Instance = this;
        }

        private void OnBecameInvisible() {
            Debug.Log("Invisible");
        }

        private void Start() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _apiController = ApiController.Instance;
            _playerData = new Player();
        }

        void Update() {
            GetPlayerInput();
            UpdateAnimator();
        }

        void LateUpdate() {
            UpdateModel();
        }

        public void DirectMovePlayer(float2 pos) {
            transform.position = (Vector2) pos;
        }

        private void FixedUpdate() {
            HandlePlayerMovement();
        }

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

        #region ModelAndAnimator

        private void UpdateModel() {
            float2 goPosition = (Vector2) transform.position;
            _playerData.Position = goPosition;
            _playerData.IsMoving = _isMoving;
            _playerData.IsRunning = _isRunning;
        }

        private void UpdateAnimator() {
            float currentX = Animator.GetFloat("X");
            float currentY = Animator.GetFloat("Y");

            if (!_isMoving && (Mathf.Approximately(currentX, 0) || Mathf.Approximately(currentY, 0))) {
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

        private void HandlePlayerMovement() {
            if (_isMoving) {
                MovePlayer(_playerInput);
            }
        }

        private void GetPlayerInput() {
            _isRunning = !Input.GetKey(SpeedModifierKey);
            _playerInput = new float2(Input.GetAxisRaw(HorizontalAxis), Input.GetAxisRaw(VerticalAxis));
            _isMoving = Mathf.Abs(_playerInput.x) + Mathf.Abs(_playerInput.y) > 0;
        }

        private void MovePlayer(float2 playerInput) {
            float playerSpeed;
            _isRunning = GetPlayerSpeed(out playerSpeed);

            float2 movePosition = CalculateMovePosition(playerInput, playerSpeed);
            UpdatePlayerSortingPosition(movePosition.y);
            _rigidbody2D.MovePosition(movePosition);
        }

        private void UpdatePlayerSortingPosition(float yPosition) {
            Vector3 position = transform.position;
            position.z = Constants.ZPositionMultiplier * yPosition + Constants.ZPositionOffset;
            transform.position = position;
        }

        private bool GetPlayerSpeed(out float playerSpeed) {
            playerSpeed = _isRunning ? RunSpeed : WalkSpeed;
            return _isRunning;
        }

        private float2 CalculateMovePosition(float2 playerInput, float playerSpeed) {
            return CalculateMoveVector(playerInput, playerSpeed) + _playerData.Position;
        }

        private float2 CalculateMoveVector(float2 playerInput, float speed) {
            return math.normalize(playerInput) * Time.fixedDeltaTime * speed;
        }

        #endregion
    }
}