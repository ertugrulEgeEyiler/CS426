using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private Text finalScoreText;
    [SerializeField] private Text maxScoreText;


    private string filePath;

    void Start()
    {
        filePath = Application.dataPath + "/Scripts/final_score.txt";

        LoadScores();
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    void LoadScores()
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length > 0)
            {
                finalScoreText.text = lines[0];
            }

            if (lines.Length > 1)
            {
                maxScoreText.text = lines[1];
            }
        }
        else
        {
            finalScoreText.text = "Final Score: 0";
            maxScoreText.text = "Max Score: 0";
        }
    }
}
