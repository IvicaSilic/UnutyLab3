using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollisionHandler : MonoBehaviour
{
    [Header("Stats")]
    public int score = 0;
    public float normalSpeed = 5f;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI statusText;

    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        UpdateScoreUI();
    }

    // Sudari s objektima
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Coin")
        {
            score += 10;
            UpdateScoreUI();
            Destroy(collision.gameObject);
            ShowStatus("Coin! +10");
        }
        else if (collision.gameObject.name == "Spike")
        {
            score -= 5;
            UpdateScoreUI();
            ShowStatus("Spike! -5 bodova");
            StartCoroutine(SpeedPenalty());
        }
    }

    // Trigger zone
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "SpeedBoostZone")
        {
            StartCoroutine(SpeedBoost());
        }
        else if (other.gameObject.name == "SlowZone")
        {
            StartCoroutine(SlowDown());
        }
    }

    // Coroutines
    IEnumerator SpeedBoost()
    {
        ShowStatus("SPEED BOOST!");
        playerController.walkSpeed = normalSpeed * 2f;
        playerController.runSpeed = normalSpeed * 3f;
        yield return new WaitForSeconds(3f);
        playerController.walkSpeed = normalSpeed;
        playerController.runSpeed = normalSpeed * 2f;
        ShowStatus("");
    }

    IEnumerator SlowDown()
    {
        ShowStatus("SLOW ZONE!");
        playerController.walkSpeed = normalSpeed * 0.5f;
        playerController.runSpeed = normalSpeed;
        yield return new WaitForSeconds(3f);
        playerController.walkSpeed = normalSpeed;
        playerController.runSpeed = normalSpeed * 2f;
        ShowStatus("");
    }

    IEnumerator SpeedPenalty()
    {
        playerController.walkSpeed = normalSpeed * 0.3f;
        yield return new WaitForSeconds(2f);
        playerController.walkSpeed = normalSpeed;
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void ShowStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }
}