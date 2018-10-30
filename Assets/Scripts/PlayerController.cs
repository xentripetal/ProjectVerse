using UnityEngine;
using Unity.Mathematics;

public class PlayerController : MonoBehaviour {
    public float walkSpeed = 2.2f;
    public float runSpeed = 4f;

    public string HorizontalAxis = "Horizontal";
    public string VerticalAxis = "Vertical";
    public KeyCode SpeedModifierKey = KeyCode.LeftShift;

    [SerializeField] public Animator animator;

    private Rigidbody2D _rigidbody2D;

    private void Start() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update() {
        HandlePlayerMovement();
    }

    #region Movement Code

    private void HandlePlayerMovement() {
        float2 playerInput = GetPlayerInput();
        bool isMoving = Mathf.Abs(playerInput.x) + Mathf.Abs(playerInput.y) > 0;

        if (isMoving) {
            MovePlayer(playerInput);
        }
        else {
            UpdateAnimator(false, false, playerInput);
        }
    }

    private float2 GetPlayerInput() {
        return new float2(Input.GetAxisRaw(HorizontalAxis), Input.GetAxisRaw(VerticalAxis));
    }

    private void MovePlayer(float2 playerInput) {
        float playerSpeed;
        bool isRunning = GetPlayerSpeed(out playerSpeed);

        Vector2 movePosition = CalculateMovePosition(playerInput, playerSpeed);
        UpdatePlayerSortingPosition(movePosition.y);
        _rigidbody2D.MovePosition(movePosition);

        UpdateAnimator(true, isRunning, playerInput);
    }

    private void UpdatePlayerSortingPosition(float yPosition) {
        Vector3 position = transform.position;
        position.z = yPosition - 100;
        transform.position = position;
    }

    private bool GetPlayerSpeed(out float playerSpeed) {
        bool isRunning = !Input.GetKey(SpeedModifierKey);
        playerSpeed = isRunning ? runSpeed : walkSpeed;
        return isRunning;
    }

    private void UpdateAnimator(bool isMoving, bool isRunning, float2 playerInput) {
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isRunning", isRunning);
        animator.SetFloat("X", playerInput.x);
        animator.SetFloat("Y", playerInput.y);
    }

    private Vector2 CalculateMovePosition(float2 playerInput, float playerSpeed) {
        return CalculateMoveVector(playerInput, playerSpeed) +
               new float2(Utils.SwapVectorDimension(transform.position));
    }

    private float2 CalculateMoveVector(float2 playerInput, float speed) {
        return math.normalize(playerInput) * Time.deltaTime * speed;
    }

    #endregion
}