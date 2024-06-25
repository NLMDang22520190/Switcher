using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;
using TMPro;

public class MidiToJsonConverter : MonoBehaviour
{
    public static MidiToJsonConverter Instance;
    public AudioSource audioSource;
    public LaneSpawn[] LaneSpawns;
    public float songDelayInSeconds;
    public double marginOfError; // in seconds
    public int inputDelayInMilliseconds;
    public int songResumeTimer = 3;

    public TextMeshProUGUI DebugText;

    public string fileLocation;
    public float noteTime;
    public float noteSpawnX;
    public float noteTapX;
    public bool isSongPlaying = false;
    public float stopSongFadeDuration = 2.0f; // Duration for fading out the song

    private int currentResumeTimer;

    public float noteDespawnX
    {
        get
        {
            return noteTapX - (noteSpawnX - noteTapX);
        }
    }

    private Player player;
    public static MidiFile midiFile;

    public SongData songData = new SongData(); // Add this line

    void Start()
    {
        Instance = this;
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(ReadFromWebsite());
        }
        else
        {
            ReadFromFile();
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource.clip == null)
        {
            Debug.LogError("No audio clip assigned to the audio source");
        }

        if (midiFile == null)
        {
            Debug.LogError("No midi file loaded");
            DebugText.text = "No midi file loaded";
        }
    }

    private IEnumerator ReadFromWebsite()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, fileLocation)))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    midiFile = MidiFile.Read(stream);
                    GetDataFromMidi();
                }
            }
        }
    }

    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();
    }

    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var LaneSpawn in LaneSpawns) LaneSpawn.SetTimeStamps(array);

        // Save note data to JSON
        SaveNoteDataToJson(Path.Combine(Application.streamingAssetsPath, "bruh.json"));

        Invoke(nameof(StartSong), songDelayInSeconds);
    }

    public void SaveNoteDataToJson(string filePath)
    {
        string json = JsonUtility.ToJson(songData, true);
        File.WriteAllText(filePath, json);
    }

    public void StartSong()
    {
        audioSource.Play();
        isSongPlaying = true;
    }

    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }
}
