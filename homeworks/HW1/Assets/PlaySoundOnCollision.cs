using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour
{
    public AudioSource audioSource;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "GameController")
        {
            audioSource.Play();
        }
    }
}
