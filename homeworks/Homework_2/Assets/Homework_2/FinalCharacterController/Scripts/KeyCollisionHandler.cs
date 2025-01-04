using UnityEngine;

public class KeyCollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door")) // Check if the collided object is tagged "Door"
        {
            Debug.Log("Key collided with the Door. Ending the game...");
            GameManager.Instance.EndGame(); // Call GameManager to end the game
        }
    }
}
