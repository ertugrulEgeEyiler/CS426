using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool hasKey = false; // Boolean to track if the player has collected the key

    // This method is triggered when a collision occurs
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object that collided has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if the collided object has the "Key" or "Rust_Key" tag
            if (collision.rigidbody.CompareTag("Key") || collision.rigidbody.CompareTag("Rust_Key"))
            {
                hasKey = true; // Set the boolean to true
                Destroy(collision.rigidbody.gameObject); // Destroy the key object
                Debug.Log("Key or Rust_Key collected and destroyed!"); // Log for debugging purposes
            }
        }
    }

    // You can use this method to check if the player has the key
    public bool HasKey()
    {
        return hasKey;
    }
}
