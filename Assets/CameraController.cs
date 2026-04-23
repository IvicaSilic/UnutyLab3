using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public float minPitch = -60f;
    public float maxPitch = 60f;

    [Header("TPS Settings")]
    public float tpsDistance = 4f;
    public float tpsHeight = 2f;
    public float tpsSmoothSpeed = 5f;
    public float tpsSideOffset = 0.8f;

    private float pitch = 0f;
    private bool isFPS = true;
    private Transform playerTransform;
    private GameObject weaponObject;

    void Start()
    {
        playerTransform = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandlePitch();
        HandleViewSwitch();

        if (!isFPS)
            HandleTPS();
    }

    void HandlePitch()
    {
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void HandleViewSwitch()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFPS = !isFPS;

            if (isFPS)
            {
                transform.localPosition = new Vector3(0f, 0.8f, 0f);
                transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
            }
            else
            {
            }
        }
    }

    void HandleTPS()
    {
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            new Vector3(tpsSideOffset, tpsHeight, -tpsDistance),
            tpsSmoothSpeed * Time.deltaTime
        );
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            Quaternion.Euler(20f, 0f, 0f),
            tpsSmoothSpeed * Time.deltaTime
        );
    }
}