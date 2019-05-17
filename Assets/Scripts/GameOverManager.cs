using System.Collections;
using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject HighScoreAlert;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    private void OnEnable()
    {
        int score = PlayerPrefs.GetInt("score");
        int highScore;
        if (PlayerPrefs.HasKey("highScore"))
        {
            highScore = PlayerPrefs.GetInt("highScore");
        }
        else 
        {
            highScore = 0; 
        }
        if (score > highScore)
        {
            PlayerPrefs.SetInt("highScore", score);
            highScoreText.text = score.ToString();
            HighScoreAlert.SetActive(true);
        }
        else
        {
            HighScoreAlert.SetActive(false);
            highScoreText.text = highScore.ToString();
        }
        scoreText.text = score.ToString();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

#if UNITY_EDITOR
    [MenuItem("Match three/Reset high score")]
    static void ResetHighScore()
    {
        PlayerPrefs.SetInt("highScore", 0);
        PlayerPrefs.SetInt("score", 0);
    }
#endif
}
