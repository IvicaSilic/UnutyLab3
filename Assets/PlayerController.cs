using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 5f;
    public float crouchHeight = 0.5f;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float staminaDrainRate = 20f;
    public float staminaRegenRate = 10f;
    public float staminaRunThreshold = 10f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;

    private Rigidbody rb;
    private float defaultHeight;
    private float currentStamina;
    private bool isGrounded;
    private float groundCheckDistance = 1.1f;
    private float yaw = 0f;

    public enum PlayerState { Idle, Walk, Run, Crouch, Jump, Fall }
    public PlayerState currentState;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        defaultHeight = transform.localScale.y;
        currentStamina = maxStamina;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CheckGrounded();
        HandleMovement();
        HandleJump();
        HandleCrouch();
        HandleStamina();
        HandleMouseLook();
        UpdateState();
    }

    void CheckGrounded()
    {
        isGrounded = Physics.SphereCast(
            transform.position,
            0.3f,
            Vector3.down,
            out RaycastHit hit,
            groundCheckDistance
        );
    }

    void HandleMovement()
    {
        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && currentStamina > staminaRunThreshold;
        float speed = isRunning ? runSpeed : walkSpeed;

        Vector3 move = transform.right * moveH + transform.forward * moveV;
        move = move.normalized * speed;

        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        // ADD THIS - clamp horizontal speed so it never exceeds current speed
        Vector3 horizontal = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (horizontal.magnitude > speed)
        {
            horizontal = horizontal.normalized * speed;
            rb.linearVelocity = new Vector3(horizontal.x, rb.linearVelocity.y, horizontal.z);
        }
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void HandleCrouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(1f, crouchHeight, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, defaultHeight, 1f);
        }
    }

    void HandleStamina()
    {
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && currentStamina > staminaRunThreshold;

        if (isRunning)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
        else
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        yaw += mouseX;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }

    void UpdateState()
    {
        float horizontalSpeed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && currentStamina > staminaRunThreshold;
        bool isCrouching = Input.GetKey(KeyCode.LeftControl);

        if (!isGrounded && rb.linearVelocity.y > 0.1f)
            currentState = PlayerState.Jump;
        else if (!isGrounded && rb.linearVelocity.y < -0.1f)
            currentState = PlayerState.Fall;
        else if (isCrouching)
            currentState = PlayerState.Crouch;
        else if (horizontalSpeed > 0.1f && isRunning)
            currentState = PlayerState.Run;
        else if (horizontalSpeed > 0.1f)
            currentState = PlayerState.Walk;
        else
            currentState = PlayerState.Idle;
    }

    public float GetStamina() => currentStamina;
    public float GetMaxStamina() => maxStamina;
}