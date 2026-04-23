using System.Collections;
using UnityEngine;
using TMPro;

public class InteractableObject : MonoBehaviour
{
    [Header("Settings")]
    public float effectDuration = 3f;
    public float bounceForce = 5f;

    [Header("UI")]
    public TextMeshProUGUI statusText;

    private bool isActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isActive)
        {
            StartCoroutine(ActivateEffect());
        }
    }

    IEnumerator ActivateEffect()
    {
        isActive = true;
        ShowStatus("Mehanizam aktiviran!");

        // Pokreni sve objekte s tagom "Interactable"
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Interactable");
        foreach (GameObject obj in objects)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(effectDuration);

        ShowStatus("");
        isActive = false;
    }

    void ShowStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }
}