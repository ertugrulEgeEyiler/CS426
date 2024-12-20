using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText;
    public Text timeText;
    private int score = 0;
    private float timeRemaining = 60f;

    void Update()
    {
        timeRemaining -= Time.deltaTime;
        timeText.text = "Time: " + Mathf.Ceil(timeRemaining).ToString();

        if (timeRemaining <= 0)
        {
            // Oyun biter
            Debug.Log("Game Over");
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score.ToString();
    }
}
