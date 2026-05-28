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
    public float tpsSmoothSpeed = 10f;
    public float tpsSideOffset = 0.8f;

    private float pitch = 0f;
    private bool isFPS = true;
    private Transform playerTransform;

    void Start()
    {
        // Uzimamo parenta (igrača) da znamo oko čega se kamera okreće
        playerTransform = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. Prvo očitaj miš (ovo mora raditi u oba moda)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // 2. Provjeri je li igrač stisnuo 'V' za promjenu pogleda
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFPS = !isFPS;
        }

        // 3. Ovisno o modu, pokreni specifičnu logiku
        if (isFPS)
        {
            ApplyFPS();
        }
        else
        {
            ApplyTPS();
        }
    }

    void ApplyFPS()
    {
        // U prvom licu kamera je fiksna na "očima"
        transform.localPosition = new Vector3(0f, 0.8f, 0f);
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void ApplyTPS()
    {
        // U trećem licu kamera glatko (Lerp) ide iza leđa
        Vector3 targetPos = new Vector3(tpsSideOffset, tpsHeight, -tpsDistance);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, tpsSmoothSpeed * Time.deltaTime);

        // I rotacija je glatka da nema onog trzanja (glitcha)
        Quaternion targetRot = Quaternion.Euler(pitch, 0f, 0f);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, tpsSmoothSpeed * Time.deltaTime);
    }
}