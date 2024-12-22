using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f; // Fare hassasiyeti
    public Transform PlayerBody; // Oyuncunun vücudu (PlayerBody)

    private float xRotation = 0f; // Yukarı-aşağı rotasyon için
    private float yRotation = 0f;
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
        yRotation -= mouseX;
        yRotation = Mathf.Clamp(yRotation, -90f,90f);
        // Kameranın yukarı-aşağı hareketi
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // Sağa-sola dönüşü PlayerBody'ye uygula (Y ekseni)
        PlayerBody.Rotate(Vector3.up * mouseX);
    }
}
