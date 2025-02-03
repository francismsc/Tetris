using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    GameObject gameOverUI;
    private void Start()
    {
        GameController.OnGameOver += OpenGameOverUI;
    }

    private void OnDestroy()
    {
        GameController.OnGameOver -= OpenGameOverUI;
    }
    public void OpenGameOverUI()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }
}
