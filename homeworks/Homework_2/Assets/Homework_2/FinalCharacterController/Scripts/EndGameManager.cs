using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class EndGameManager : MonoBehaviour
{
    // Function to restart the game and load Scene 0
    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        SceneManager.LoadScene(0); // Load Scene 0 (assumed to be your main game scene)
    }

    // Function to quit the game
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); // Quit the application
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
#endif
    }
}
