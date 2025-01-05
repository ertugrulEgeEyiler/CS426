using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class GameManager : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;
        ResetGame(); // Oyuna başlarken tüm değerleri sıfırla
    }

    public static GameManager Instance; // Singleton pattern

    [Header("Player Health")]
    public int playerHealth = 100; // Initial health
    public bool isGameOver = false;

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

    // Oyunu sıfırlayan metot
    public void ResetGame()
    {
        playerHealth = 100; // Oyuncunun sağlığını sıfırla
        isGameOver = false; // Game Over durumunu sıfırla
        hasKey = false; // Anahtar durumunu sıfırla
        Debug.Log("Game reset. Player health set to 100.");
    }

    // Decrease player health
    public void DecreaseHealth(int amount)
    {
        if (isGameOver) return; // Game over durumunda hasar alınmaz

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
        if (!isGameOver)
        {
            isGameOver = true;
            Debug.Log("Game Over! Sending to Scene 1...");
            SceneManager.LoadScene(1); // Load Scene 1
        }
    }

    // Handle key collection
    public void CollectKey(GameObject keyObject)
    {
        if (keyObject != null)
        {
            hasKey = true;
            Debug.Log("Key collected!");
            Destroy(keyObject); // Anahtarı yok et
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

    // Restart butonu ile oyun sıfırlama
    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        ResetGame(); // Tüm değişkenleri sıfırla
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Şu anki sahneyi yeniden yükle
    }
}
