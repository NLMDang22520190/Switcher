using System.Collections;
using TMPro;
using UnityEngine;

public class NormalEventManager : MonoBehaviour
{
    private const string HOME_SCENE = "Main Screen";

    public static NormalEventManager Instance { get; private set; }

    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject completeMenu;
    [SerializeField] private GameObject gamePlayUI;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI highestScoreText;
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource gameOverSFX;

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

        pauseMenu.SetActive(false);
        completeMenu.SetActive(false);
    }

    public void Pause()
    {
        if (!isPaused)
        {
            isPaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            Player.Instance.Pause();
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        Player.Instance.Resume();
        isPaused = false;
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
        NormalSpawnPoint.Instance.CanSpawn = false;
        GameEnded();
    }

    private void OnDestroy()
    {
        if (healthSystem != null)
        {
            healthSystem.OnDeath -= HandleDeath;
        }
    }

    private void HandleSongEnd()
    {
        // Add implementation if needed
    }

    private void GameEnded()
    {
        completeMenu.SetActive(true);
        pauseButton.SetActive(false);
        bgm.Stop();
        gameOverSFX.Play();

        int score = NormalScoreManager.Instance.Score;
        int combo = NormalScoreManager.Instance.CurrentHighestCombo;
        int highestScore = NormalScoreManager.Instance.HighestScore;
        NormalScoreManager.Instance.CheckForNewHighScore();
        
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
}
