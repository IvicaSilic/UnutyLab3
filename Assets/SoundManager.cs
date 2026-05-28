using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Sounds")]
    public AudioClip footstepSound;
    public AudioClip jumpSound;

    private AudioSource audioSource;
    private PlayerController playerController;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        // Stop footstep sound when not walking
        if (playerController.currentState == PlayerController.PlayerState.Idle ||
            playerController.currentState == PlayerController.PlayerState.Jump)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }

    public void PlayFootstep()
    {
        if (footstepSound != null && !audioSource.isPlaying)
            audioSource.PlayOneShot(footstepSound);
    }

    public void PlayJump()
    {
        Debug.Log("PlayJump called! Sound is: " + jumpSound);
        if (jumpSound != null)
            audioSource.PlayOneShot(jumpSound);
        else
            Debug.Log("Jump sound is NULL!");
    }
}