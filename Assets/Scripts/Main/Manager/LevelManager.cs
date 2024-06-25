using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private GameObject loaderCanvas;
    [SerializeField] private Image loadingBar;
    [SerializeField] private TextMeshProUGUI loadingText;

    private float target;
    private string[] loadingMessages = { "loading.", "loading..", "loading..." };
    private int loadingMessageIndex = 0;
    private float loadingTextUpdateInterval = 0.3f;

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

        Time.timeScale = 1;
    }

    public async void LoadLevel(string levelName)
    {
        target = 0;
        loadingBar.fillAmount = 0;

        var scene = SceneManager.LoadSceneAsync(levelName);
        scene.allowSceneActivation = false;

        loaderCanvas.SetActive(true);

        StartCoroutine(UpdateLoadingText());

        do
        {
            await Task.Delay(1000);
            loadingBar.fillAmount = scene.progress;
            target = scene.progress;
        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        loaderCanvas.SetActive(false);

        StopCoroutine(UpdateLoadingText());
    }

    private void Update()
    {
        loadingBar.fillAmount = Mathf.MoveTowards(loadingBar.fillAmount, target, Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            LoadLevel("Main Screen");
        }
    }

    private IEnumerator UpdateLoadingText()
    {
        while (true)
        {
            loadingText.text = loadingMessages[loadingMessageIndex];
            loadingMessageIndex = (loadingMessageIndex + 1) % loadingMessages.Length;
            yield return new WaitForSeconds(loadingTextUpdateInterval);
        }
    }

    public void LoadMainScene()
    {
        Time.timeScale = 1;
        LoadLevel("Main Screen");
    }

    public void LoadNormalMode()
    {
        Time.timeScale = 1;
        LoadLevel("Normal Map");
    }

    public void CloseApp()
    {
        Application.Quit();
    }

}
