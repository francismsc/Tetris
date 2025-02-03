using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    int score = 0;
    int linesCleared = 0;
    int totalLinesCleared = 0;
    int level = 1;

    [SerializeField]
    const int clearsPerLevel = 1;


    public event Action<int> OnScoreChanged;
    public event Action<int> OnLevelChanged;
    public event Action<int> OnLevelUpChanged;
    public event Action<int> OnLinesChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one LevelGrid! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        LineClearingManager.OnLinesCleared += CalculateLineClearScore;
        LineClearingManager.OnLinesCleared += UpdateLevel;
    }

    private void OnDisable()
    {
        LineClearingManager.OnLinesCleared -= CalculateLineClearScore;
        LineClearingManager.OnLinesCleared -= UpdateLevel;
    }

    private void AddScore(int addScore)
    {
        score += addScore;
        OnScoreChanged?.Invoke(score);
    }

    private void AddLevel(int newLevel)
    {
        level += newLevel;
        OnLevelChanged?.Invoke(level);
        OnLevelUpChanged?.Invoke(newLevel);
    }

    private void CalculateLineClearScore(int clearedLines)
    {
        int pointsToAdd = (clearedLines ^ 2) * 100;
        AddScore(pointsToAdd); 
        AddLines(clearedLines);
    }

    private void UpdateLevel(int clearedLines)
    {
        if (linesCleared >= clearsPerLevel)
        {
            int levelUp = linesCleared / clearsPerLevel;
            int remainder = linesCleared % clearsPerLevel;

            linesCleared = remainder;
            AddLevel(levelUp);
        }
    }

    private void AddLines(int clearedLines)
    {
        totalLinesCleared += clearedLines;
        OnLinesChanged?.Invoke(totalLinesCleared);
    }


}
