using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public bool isGameOver;
    [SerializeField] GameObject gameOverPanel;

    private void Start()
    {
        isGameOver = false;
        // Time.timeScale = 1;
    }

    public void GameOver()
    {
        isGameOver = true;
        Cursor.lockState = CursorLockMode.None; // 取消鼠标锁定状态，要不然不能点击按钮
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        isGameOver = false;
        SceneManager.LoadScene("Level");
    }
}