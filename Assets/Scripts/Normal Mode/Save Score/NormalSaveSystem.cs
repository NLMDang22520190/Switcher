using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NormalSaveSystem
{
    private static string normalPath = Path.Combine(Application.persistentDataPath, "normalmode_scoredata.json");

    public static void SaveScore(int highestScore, int highestCombo)
    {
        NormalScoreData data = new NormalScoreData(highestScore, highestCombo);
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(normalPath, json);
    }

    public static NormalScoreData LoadScore(bool isRhythmMode)
    {
        if (File.Exists(normalPath))
        {
            string json = File.ReadAllText(normalPath);
            NormalScoreData data = JsonUtility.FromJson<NormalScoreData>(json);
            return data;
        }
        // If file does not exist, create a new one
        else
        {
            SaveScore(0, 0);
            return new NormalScoreData(0, 0);
        }
    }
}
