using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [Header("Pause Input")]
    [SerializeField] [Tooltip("Key used to toggle pause during gameplay.")]
    private KeyCode pauseKey = KeyCode.Escape;

    [SerializeField] [Tooltip("If enabled, the game starts in a paused state.")]
    private bool startPaused = false;

    [Header("Optional UI")]
    [SerializeField] [Tooltip("Optional pause menu panel that is shown while the game is paused.")]
    private GameObject pausePanel;

    [Header("Runtime Debug")]
    [SerializeField] [Tooltip("True while the game is currently paused.")]
    private bool isPaused;

    private void Awake()
    {
        SetPaused(startPaused);
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        SetPaused(!isPaused);
    }

    public void ResumeGame()
    {
        SetPaused(false);
    }

    public void PauseGame()
    {
        SetPaused(true);
    }

    private void SetPaused(bool paused)
    {
        isPaused = paused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(isPaused);
        }
    }

    private void OnDestroy()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
    }
}
