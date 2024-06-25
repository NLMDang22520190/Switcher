using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NormalScoreManager : MonoBehaviour
{
    public static NormalScoreManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image delayedHealthBar;
    [SerializeField] private bool isRhythmMode;

    private HealthSystem healthSystem;

    private int score;
    private int displayScore;
    private int combo;
    private int currentHighestCombo;

    private float currentHealthPercent;
    private float delayedHealthPercent;

    private int highestScore;
    private int highestCombo;

    public int Score
    {
        get => score;
        private set
        {
            score = value;
            OnScoreChanged?.Invoke(score); // Trigger event when score changes
        }
    }
    public int Combo { get => combo; set => combo = value; }
    public int HighestScore { get => highestScore; set => highestScore = value; }
    public int CurrentHighestCombo { get => currentHighestCombo; set => currentHighestCombo = value; }

    public event Action<int> OnScoreChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Score = 0;
        displayScore = 0;
        Combo = 0;
        CurrentHighestCombo = 0;

        healthSystem = Player.Instance.GetComponent<HealthSystem>();
        healthSystem.OnHealthChanged += HandleHealthChanged;

        LoadHighScore();
    }

    private void Update()
    {
        DisplayScore();
    }

    public void SetMode(bool rhythmMode)
    {
        isRhythmMode = rhythmMode;
        LoadHighScore();
    }

    public void AddScore(int value)
    {
        Score += value;
    }

    public void AddCombo()
    {
        Combo++;
        if (Combo > CurrentHighestCombo)
        {
            CurrentHighestCombo = Combo;
        }
    }

    public void ResetCombo()
    {
        Combo = 0;
    }

    private void DisplayScore()
    {
        displayScore = (int)Mathf.Lerp(displayScore, Score, 0.05f);
        if (Mathf.Abs(displayScore - Score) < 50)
        {
            displayScore = Score;
        }
        scoreText.text = displayScore.ToString("D9");
        comboText.text = Combo.ToString();
    }

    private void HandleHealthChanged(int currentHealth)
    {
        currentHealthPercent = healthSystem.GetHealthPercentage();
        StartCoroutine(UpdateDelayedHealthBar());
        healthBar.fillAmount = currentHealthPercent;
    }

    private IEnumerator UpdateDelayedHealthBar()
    {
        while (Mathf.Abs(delayedHealthPercent - currentHealthPercent) > 0.01f)
        {
            delayedHealthPercent = Mathf.Lerp(delayedHealthPercent, currentHealthPercent, 0.05f);
            delayedHealthBar.fillAmount = delayedHealthPercent;
            yield return null;
        }
        delayedHealthPercent = currentHealthPercent;
        delayedHealthBar.fillAmount = delayedHealthPercent;
    }

    public void CheckForNewHighScore()
    {
        if (Score > HighestScore)
        {
            HighestScore = Score;
            highestCombo = CurrentHighestCombo;
            SaveHighScore();
        }
    }

    private void SaveHighScore()
    {
        NormalSaveSystem.SaveScore(HighestScore, highestCombo);
    }

    private void LoadHighScore()
    {
        NormalScoreData data = NormalSaveSystem.LoadScore(isRhythmMode);
        HighestScore = data.highestScore;
        highestCombo = data.highestCombo;

        Debug.Log("Loaded high score: " + HighestScore);
        Debug.Log("Loaded highest combo: " + highestCombo);
    }
}
