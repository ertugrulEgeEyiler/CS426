using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton pattern

    [Header("Player Health")]
    public int playerHealth = 100; // Initial health
    private bool isGameOver = false;

    private bool hasKey = false; // Player's key status

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevent destruction on scene load
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Decrease player health
    public void DecreaseHealth(int amount)
    {
        if (isGameOver) return; // Prevent further damage after game over

        playerHealth -= amount;
        Debug.Log("Player Health: " + playerHealth);

        if (playerHealth <= 0)
        {
            GameOver();
        }
    }

    // Handle game over logic
    private void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over! Sending to Scene 1...");
        SceneManager.LoadScene(1); // Load Scene 1
    }

    // Handle key collection
    public void CollectKey(GameObject keyObject)
    {
        if (keyObject != null)
        {
            hasKey = true;
            Debug.Log("Key collected!");
            Destroy(keyObject); // Destroy the key object
        }
        else
        {
            Debug.LogError("Key object is null! Unable to collect key.");
        }
    }

    // Check if the player has the key
    public bool HasKey()
    {
        return hasKey;
    }

    // Handle door interaction
    public void OpenDoor()
    {
        if (hasKey)
        {
            Debug.Log("Door opened, loading Scene 1...");
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.Log("You need a key to open the door!");
        }
    }
    public void EndGame()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Debug.Log("Key collided with the door. Game Over!");
            SceneManager.LoadScene(1); // Load the game over scene (Scene 1)
        }
    }

}
