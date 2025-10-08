using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject pausePanel;

    [Header("Buttons")]
    public Button resumeButton;
    public Button mainMenuButton;
    public Button exitButton;

    [Header("Settings")]
    public string mainMenuSceneName = "MainMenu";
    public KeyCode pauseKey = KeyCode.Escape;

    private bool isPaused = false;

    void Start()
    {
        // Setup button listeners
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);

        // Ensure panel is hidden at start
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    void Update()
    {
        // Check for pause input
        if (Input.GetKeyDown(pauseKey))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Freeze game time

        // Show pause panel
        if (pausePanel != null)
            pausePanel.SetActive(true);

        // Optional: Disable player input
        // Optional: Show cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume game time

        // Hide pause panel
        if (pausePanel != null)
            pausePanel.SetActive(false);

        // Optional: Enable player input
        // Optional: Hide cursor (for FPS games)
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("Game Resumed");
    }

    public void GoToMainMenu()
    {
        // Resume time before loading new scene
        Time.timeScale = 1f;

        // Load main menu scene
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogWarning("Main menu scene name not set!");
        }
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");

        // Resume time before exiting (good practice)
        Time.timeScale = 1f;

        // Exit the application
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Public method to check pause state
    public bool IsGamePaused()
    {
        return isPaused;
    }

    // Toggle pause state
    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    // For external scripts to check if game is paused
    public static bool IsPaused()
    {
        return Time.timeScale == 0f;
    }
}