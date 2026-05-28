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
    private Coroutine speedRoutine;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        UpdateScoreUI();
    }

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
            if (speedRoutine != null) StopCoroutine(speedRoutine);
            speedRoutine = StartCoroutine(SpeedPenalty());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("PROŠAO KROZ: " + other.gameObject.name + " | Tag: " + other.gameObject.tag);

        if (other.gameObject.name == "SpeedBoostZone")
        {
            Debug.Log("!!! AKTIVIRAN SPEED BOOST !!!");
            if (speedRoutine != null) StopCoroutine(speedRoutine);
            speedRoutine = StartCoroutine(SpeedBoost());
        }
        else if (other.gameObject.name == "SlowBoostZone") // FIXED NAME
        {
            if (speedRoutine != null) StopCoroutine(speedRoutine);
            speedRoutine = StartCoroutine(SlowDown());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "SpeedBoostZone" || other.gameObject.name == "SlowBoostZone")
        {
            if (speedRoutine != null) StopCoroutine(speedRoutine);
            playerController.walkSpeed = normalSpeed;
            playerController.runSpeed = normalSpeed * 2f;
            ShowStatus("Izašao iz zone");
            StartCoroutine(ClearStatus());
        }
    }

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

    IEnumerator ClearStatus()
    {
        yield return new WaitForSeconds(1.5f);
        ShowStatus("");
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