using UnityEngine;

public class DustEffect : MonoBehaviour
{
    public ParticleSystem dustParticles;
    private PlayerController playerController;
    private PlayerController.PlayerState previousState;

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        previousState = playerController.currentState;
    }

    void Update()
    {
        PlayerController.PlayerState currentState = playerController.currentState;

        // Check if we just transitioned FROM Jump/Fall TO Idle/Walk/Run
        bool wasInAir = previousState == PlayerController.PlayerState.Jump ||
                        previousState == PlayerController.PlayerState.Fall;

        bool isOnGround = currentState == PlayerController.PlayerState.Idle ||
                          currentState == PlayerController.PlayerState.Walk ||
                          currentState == PlayerController.PlayerState.Run;

        if (wasInAir && isOnGround)
        {
            dustParticles.Play();
        }

        previousState = currentState;
    }
}