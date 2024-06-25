using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicGameManager : MonoBehaviour
{
    private static MusicGameManager instance;
    public static MusicGameManager Instance { get => instance; }

    private Player player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void Restart()
    {
        LevelManager.Instance.LoadLevel(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
