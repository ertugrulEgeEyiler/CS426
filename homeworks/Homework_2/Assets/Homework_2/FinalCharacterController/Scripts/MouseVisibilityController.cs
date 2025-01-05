using UnityEngine;

public class MouseVisibilityController : MonoBehaviour
{
    private bool isCursorVisible = false; // Mouse başlangıçta gizli

    void Start()
    {
        UpdateCursorState();
    }

    void Update()
    {
        // Space tuşuna basıldığında mouse'un görünürlüğü değiştirilir
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCursorVisible = !isCursorVisible; // Mevcut durumu tersine çevir
            UpdateCursorState();
        }
    }

    void UpdateCursorState()
    {
        if (isCursorVisible)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; // Mouse serbest
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; // Mouse ekranın ortasına kilitlenir
        }
    }
}
