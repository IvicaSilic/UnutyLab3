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
            Debug.Log("E pressed!");
            StartCoroutine(ActivateEffect());
        }
    }

    IEnumerator ActivateEffect()
    {
        isActive = true;
        ShowStatus("Mehanizam aktiviran!");

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Interactable");
        Debug.Log("Pronadeno objekata: " + objects.Length);

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