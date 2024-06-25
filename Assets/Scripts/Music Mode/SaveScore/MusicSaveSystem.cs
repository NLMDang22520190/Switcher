using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MusicSaveSystem : MonoBehaviour
{
    private static string path = Path.Combine(Application.persistentDataPath, "musicmode_scoredata.json");

    public static void SaveScore(int highestScore, int highestCombo, bool isFullCombo)
    {
        MusicScoreData data = new MusicScoreData(highestScore, highestCombo, isFullCombo);
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(path, json);
    }

    public static MusicScoreData LoadScore()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            MusicScoreData data = JsonUtility.FromJson<MusicScoreData>(json);
            return data;
        }
        // If file does not exist, create a new one
        else
        {
            SaveScore(0, 0, false);
            return new MusicScoreData(0, 0, false);
        }
    }
}
