using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerController;
    private Rigidbody rb;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float inputMagnitude = new Vector2(h, v).magnitude;

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && playerController.GetStamina() > 10f;

        float speed = inputMagnitude > 0.1f ? 0.5f : 0f;
        if (isRunning && inputMagnitude > 0.1f) speed = 1f;

        animator.SetFloat("Speed", speed);
        animator.SetBool("IsJumping", playerController.currentState == PlayerController.PlayerState.Jump);
        animator.SetBool("IsCrouching", playerController.currentState == PlayerController.PlayerState.Crouch);

        // DEBUG
        Debug.Log("IsJumping: " + (playerController.currentState == PlayerController.PlayerState.Jump));
    }
}