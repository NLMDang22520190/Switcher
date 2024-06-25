using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreenGame : MonoBehaviour
{
    private const string SONG_SCENE = "MusicMode";
    private const string NORMAl_SCENE = "NormalMode";

    public void LoadSongMode()
    {
        LevelManager.Instance.LoadLevel(SONG_SCENE);
    }

    public void LoadNormalMode()
    {
        LevelManager.Instance.LoadLevel(NORMAl_SCENE);
    }
}
