using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private string scoreFormat = "Score: {0}";

    private void OnEnable()
    {
        KillCount.CountChanged += UpdateScore;
        UpdateScore(KillCount.GetKilledCount());
    }

    private void OnDisable()
    {
        KillCount.CountChanged -= UpdateScore;
    }

    private void UpdateScore(int killedCount)
    {
        if (scoreText != null)
        {
            scoreText.text = string.Format(scoreFormat, killedCount);
        }
    }
}