using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NoraGameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TMP_Text endText;
    [SerializeField] private Button restartButton;

    [Header("Timer")]
    [SerializeField] private float roundTime = 60f;

    private int score;
    private float timeLeft;
    private bool gameOver;

    private void Awake()
    {
        timeLeft = roundTime;
        score = 0;
        gameOver = false;

        if (endPanel != null)
        {
            endPanel.SetActive(false);
        }

        UpdateScoreUI();
        UpdateTimerUI();

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartScene);
        }
    }

    private void Update()
    {
        if (gameOver)
        {
            return;
        }

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            GameOver("Time's up!");
            return;
        }

        UpdateTimerUI();
    }

    public void AddScore(int amount)
    {
        if (gameOver)
        {
            return;
        }

        score += amount;
        UpdateScoreUI();
    }

    public void LoseTime(float amount)
    {
        if (gameOver)
        {
            return;
        }

        timeLeft = Mathf.Max(0f, timeLeft - amount);
        UpdateTimerUI();

        if (timeLeft <= 0f)
        {
            GameOver("Time's up!");
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = $"Time: {Mathf.CeilToInt(timeLeft)}";
        }
    }

    private void GameOver(string message)
    {
        gameOver = true;

        if (endText != null)
        {
            endText.text = message;
        }

        if (endPanel != null)
        {
            endPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    private void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}