using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI linesText;

    void Start()
    {
        ScoreManager_OnLevelChanged(1);
        ScoreManager_OnScoreChanged(0);
        ScoreManager_OnLinesChanged(0);

        ScoreManager.Instance.OnLevelChanged += ScoreManager_OnLevelChanged;
        ScoreManager.Instance.OnScoreChanged += ScoreManager_OnScoreChanged;
        ScoreManager.Instance.OnLinesChanged += ScoreManager_OnLinesChanged;
    }

    private void OnDestroy()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnLevelChanged -= ScoreManager_OnLevelChanged;
            ScoreManager.Instance.OnScoreChanged -= ScoreManager_OnScoreChanged;
            ScoreManager.Instance.OnLinesChanged -= ScoreManager_OnLinesChanged;
        }
    }


    private void ScoreManager_OnLevelChanged(int newLevel)
    {
        levelText.text = "Level "+ newLevel.ToString();
    }

    private void ScoreManager_OnScoreChanged(int newScore)
    {
        scoreText.text ="Score "+ newScore.ToString();
    }

    private void ScoreManager_OnLinesChanged(int newLines)
    {
        linesText.text = "Lines Cleared " + newLines.ToString();
    }
}
