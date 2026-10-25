using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    private int score = 0;
    public GameObject winPanel;
    public void AddScore()
    {
        score++;
        scoreText.text = "Your score : " + score;
        if (score >= 6)
        {
            winPanel.SetActive(true);
        }
    }
}
