using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f; // Fare hassasiyeti
    public Transform PlayerBody; // Oyuncunun vücudu (PlayerBody)

    private float xRotation = 0f; // Yukarı-aşağı rotasyon için

    void Start()
    {
        // Fareyi ekranın merkezine kilitle
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Fare hareketlerini al
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Yukarı-aşağı bakışı kontrol et (X ekseni)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Yukarı-aşağı sınırlandırma

        // Kameranın yukarı-aşağı hareketi
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Sağa-sola dönüşü PlayerBody'ye uygula (Y ekseni)
        PlayerBody.Rotate(Vector3.up * mouseX);
    }
}
