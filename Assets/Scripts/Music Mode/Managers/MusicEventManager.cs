using System.Collections;
using TMPro;
using UnityEngine;

public class MusicEventManager : MonoBehaviour
{
    private const string HOME_SCENE = "Main Screen";

    public static MusicEventManager Instance { get; private set; }

    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject completeMenu;
    [SerializeField] private GameObject gamePlayUI;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI highestScoreText;

    private HealthSystem healthSystem;



    private bool isPaused = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        healthSystem = Player.Instance.GetComponent<HealthSystem>();

        healthSystem.OnDeath += HandleDeath;
        SongManager.Instance.OnSongEnd += HandleSongEnd;

        pauseMenu.SetActive(false);
        completeMenu.SetActive(false);
    }

    public void Pause()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
            pauseMenu.SetActive(true);
            SongManager.Instance.Pause();
            Player.Instance.Pause();
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        Player.Instance.Resume();
        isPaused = false;
        SongManager.Instance.Resume();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        LevelManager.Instance.LoadLevel(currentScene);
    }

    private void HandleDeath()
    {
        gamePlayUI.SetActive(false);
        Player.Instance.Pause();
        Player.Instance.GetComponent<Collider>().enabled = false;
        Failed();
    }

    private void HandleSongEnd()
    {
        Complete("Complete");
    }

    private void Failed()
    {
        SongManager.Instance.Failed();

        title.text = "Failed";
        completeMenu.SetActive(true);
        gamePlayUI.SetActive(false);
        pauseButton.SetActive(false);

        LoadScore();
    }

    private void Complete(string state)
    {
        title.text = state;
        completeMenu.SetActive(true);
        gamePlayUI.SetActive(false);
        pauseButton.SetActive(false);

        LoadScore();
    }

    private void LoadScore()
    {
        int score = MusicScoreManager.Instance.Score;
        int combo = MusicScoreManager.Instance.CurrentHighestCombo;
        int highestScore = MusicScoreManager.Instance.HighestScore;
        MusicScoreManager.Instance.CheckForNewHighScore();

        StartCoroutine(UpdateScoreText(score, highestScore, combo));
    }

    public void LoadHomeScene()
    {
        LevelManager.Instance.LoadLevel(HOME_SCENE);
    }

    private IEnumerator UpdateScoreText(int finalScore, int highestScore, int finalCombo)
    {
        float baseDuration = 1.0f; // Base duration for the animation
        float scoreFactor = 0.0f; // Additional duration per score point
        float duration = baseDuration + finalScore * scoreFactor;

        float elapsedTime = 0f;
        int initialScore = 0;

        highestScoreText.text = highestScore.ToString();

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            int currentScore = Mathf.RoundToInt(Mathf.Lerp(initialScore, finalScore, elapsedTime / duration));
            scoreText.text = currentScore.ToString("D9");
            yield return null;
        }

        // Ensure the final score and combo are correctly displayed
        scoreText.text = finalScore.ToString("D9");
        comboText.text = finalCombo.ToString();
    }

    private void OnDestroy()
    {
        if (healthSystem != null)
        {
            healthSystem.OnDeath -= HandleDeath;
        }

        if (SongManager.Instance != null)
        {
            SongManager.Instance.OnSongEnd -= HandleSongEnd;
        }
    }
}
