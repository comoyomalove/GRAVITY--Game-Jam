using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [Header("Scene Loading")]
    [SerializeField] [Tooltip("Name of the gameplay scene to load when the Play button is pressed. This must exactly match the scene name in Build Settings.")]
    private string gameplaySceneName = "NoraScene";

    [SerializeField] [Tooltip("If enabled, the game will log detailed button and scene loading debug information.")]
    private bool verboseDebugLogs = true;

    [Header("Optional Button Auto-Wire")]
    [SerializeField] [Tooltip("Optional Play button reference. If assigned, this script will automatically bind PlayGame() on Awake.")]
    private Button playButton;

    [SerializeField] [Tooltip("Optional Quit button reference. If assigned, this script will automatically bind QuitGame() on Awake.")]
    private Button quitButton;

    private void Awake()
    {
        if (playButton != null)
        {
            playButton.onClick.RemoveListener(PlayGame);
            playButton.onClick.AddListener(PlayGame);
            if (verboseDebugLogs)
            {
                Debug.Log("TitleScreen: Play button auto-wired to PlayGame().", this);
            }
        }
        else if (verboseDebugLogs)
        {
            Debug.LogWarning("TitleScreen: No Play button assigned. Use Button OnClick manually or assign playButton in Inspector.", this);
        }

        if (quitButton != null)
        {
            quitButton.onClick.RemoveListener(QuitGame);
            quitButton.onClick.AddListener(QuitGame);
            if (verboseDebugLogs)
            {
                Debug.Log("TitleScreen: Quit button auto-wired to QuitGame().", this);
            }
        }
    }

    public void PlayGame()
    {
        if (verboseDebugLogs)
        {
            Debug.Log($"TitleScreen: PlayGame() called. Attempting to load scene '{gameplaySceneName}'.", this);
        }

        if (string.IsNullOrWhiteSpace(gameplaySceneName))
        {
            Debug.LogError("TitleScreen: Gameplay scene name is empty. Please assign a valid scene name in the Inspector.", this);
            return;
        }

        SceneManager.LoadScene(gameplaySceneName);
    }

    public void QuitGame()
    {
        if (verboseDebugLogs)
        {
            Debug.Log("TitleScreen: QuitGame() called.", this);
        }

        Application.Quit();
    }
}
