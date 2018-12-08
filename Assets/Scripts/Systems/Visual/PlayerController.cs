using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Verse.Systems.Visual {
    public class PlayerController : MonoBehaviour {
        [FormerlySerializedAs("walkSpeed")] public float WalkSpeed = 2.2f;
        [FormerlySerializedAs("runSpeed")] public float RunSpeed = 4f;
        public static PlayerController Instance;

        public string HorizontalAxis = "Horizontal";
        public string VerticalAxis = "Vertical";
        public KeyCode SpeedModifierKey = KeyCode.LeftShift;

        private Player playerData;
        private float2 playerInput;
        private bool isMoving;
        private bool isRunning;

        [SerializeField] public Animator animator;

        private Rigidbody2D _rigidbody2D;
        private APIController _apiController;

        void Awake() {
            Instance = this;
        }

        private void OnBecameInvisible() {
            Debug.Log("Invisible");
        }

        private void Start() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _apiController = APIController.Instance;
            playerData = new Player();
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
                _apiController.OnPlayerEnter(playerData, other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.tag == "TransparencyCollider") {
                var sr = other.GetComponentInParent<SpriteRenderer>();
                sr.color = ToggleOpacity(sr.color);
            }
        }

        private Color ToggleOpacity(Color color) {
            if (color.a == 1) {
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
            playerData.position = goPosition;
            playerData.isMoving = isMoving;
            playerData.isRunning = isRunning;
        }

        private void UpdateAnimator() {
            float currentX = animator.GetFloat("X");
            float currentY = animator.GetFloat("Y");

            if (!isMoving && (currentX != 0 || currentY != 0)) {
                animator.SetFloat("lastX", currentX);
                animator.SetFloat("lastY", currentY);
            }

            animator.SetBool("isMoving", isMoving);
            animator.SetBool("isRunning", isRunning);
            animator.SetFloat("X", playerInput.x);
            animator.SetFloat("Y", playerInput.y);
        }

        #endregion

        #region Movement Code

        private void HandlePlayerMovement() {
            if (isMoving) {
                MovePlayer(playerInput);
            }
        }

        private void GetPlayerInput() {
            isRunning = !Input.GetKey(SpeedModifierKey);
            playerInput = new float2(Input.GetAxisRaw(HorizontalAxis), Input.GetAxisRaw(VerticalAxis));
            isMoving = Mathf.Abs(playerInput.x) + Mathf.Abs(playerInput.y) > 0;
        }

        private void MovePlayer(float2 playerInput) {
            float playerSpeed;
            bool isRunning = GetPlayerSpeed(out playerSpeed);

            float2 movePosition = CalculateMovePosition(playerInput, playerSpeed);
            UpdatePlayerSortingPosition(movePosition.y);
            _rigidbody2D.MovePosition(movePosition);
        }

        private void UpdatePlayerSortingPosition(float yPosition) {
            Vector3 position = transform.position;
            position.z = Utilities.Constants.zPositionMultiplier * yPosition + Utilities.Constants.zPositionOffset;
            transform.position = position;
        }

        private bool GetPlayerSpeed(out float playerSpeed) {
            playerSpeed = isRunning ? RunSpeed : WalkSpeed;
            return isRunning;
        }

        private float2 CalculateMovePosition(float2 playerInput, float playerSpeed) {
            return CalculateMoveVector(playerInput, playerSpeed) + playerData.position;
        }

        private float2 CalculateMoveVector(float2 playerInput, float speed) {
            return math.normalize(playerInput) * Time.fixedDeltaTime * speed;
        }

        #endregion
    }
}