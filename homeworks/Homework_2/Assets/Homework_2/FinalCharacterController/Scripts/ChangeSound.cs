using UnityEngine;
using UnityEngine.UI;

public class ChangeSound : MonoBehaviour
{
    public Button button;
    private bool isOn = true;

    public AudioSource audioSource;

    public void ButtonClicked()
    {
        if (isOn)
        {
            isOn = false;
            audioSource.mute = true;
        }
        else
        {
            isOn = true;
            audioSource.mute = false;
        }
    }
}
