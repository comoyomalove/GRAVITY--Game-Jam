using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [Header("Scene Loading")]
    [SerializeField] [Tooltip("Name of the gameplay scene to load when the Play button is pressed. This must exactly match the scene name in Build Settings.")]
    private string gameplaySceneName = "NoraScene";

    [SerializeField] [Tooltip("Optional Play button. If assigned, this script will automatically wire the click event on Awake.")]
    private Button playButton;

    [SerializeField] [Tooltip("Optional Quit button. If assigned, this script will automatically wire the click event on Awake.")]
    private Button quitButton;

    [SerializeField] [Tooltip("If enabled, logs useful debug messages when buttons are wired and pressed.")]
    private bool verboseLogging = true;

    private void Awake()
    {
        if (playButton != null)
        {
            playButton.onClick.RemoveListener(PlayGame);
            playButton.onClick.AddListener(PlayGame);
            if (verboseLogging) Debug.Log("TitleScreen: Play button wired.", this);
        }

        if (quitButton != null)
        {
            quitButton.onClick.RemoveListener(QuitGame);
            quitButton.onClick.AddListener(QuitGame);
            if (verboseLogging) Debug.Log("TitleScreen: Quit button wired.", this);
        }
    }

    public void PlayGame()
    {
        if (string.IsNullOrWhiteSpace(gameplaySceneName))
        {
            Debug.LogError("TitleScreen: Gameplay scene name is empty.", this);
            return;
        }

        if (verboseLogging) Debug.Log($"TitleScreen: Loading scene '{gameplaySceneName}'.", this);
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void QuitGame()
    {
        if (verboseLogging) Debug.Log("TitleScreen: Quit pressed.", this);
        Application.Quit();
    }
}
