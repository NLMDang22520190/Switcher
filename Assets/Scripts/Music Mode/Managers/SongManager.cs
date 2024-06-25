using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Security.Cryptography;

public class SongManager : MonoBehaviour
{


    [Header("Song Data")]
    public static SongManager Instance;
    public AudioSource audioSource;
    public AudioClip songFailSFX;
    public float stopSongFadeDuration;
    public MusicLane[] lanes;
    public float songDelayInSeconds;

    [Header("Gameplay Data")]
    public double marginOfError; // in seconds
    public double nearMarginOfError; // in seconds
    public int inputDelayInMilliseconds;
    public float songResumeTimer;

    [Header("JSON Data")]
    public string fileLocation;

    [Header("Note Data")]
    public float noteTime;
    public NoteDirection noteDirection;
    public float noteSpawn;
    public float noteTap;

    [Header("Gameplay Data")]
    public bool isPause;

    private float currentResumeTimer;

    public event System.Action OnSongEnd;

    public float noteDespawnY
    {
        get
        {
            return noteTap - (noteSpawn - noteTap);
        }
    }

    public float SongResumeTimer { get => songResumeTimer; set => songResumeTimer = value; }

    private SongData songData;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        StartCoroutine(ReadFromFile());
    }

    private void Start()
    {
        audioSource.Stop();
        Invoke(nameof(EndSong), audioSource.clip.length + songDelayInSeconds + 2);
    }

    private IEnumerator ReadFromFile()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileLocation);

        string json;
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            using (UnityWebRequest www = UnityWebRequest.Get(filePath))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.LogError(www.error);
                    yield break;
                }
                json = www.downloadHandler.text;
            }
        }
        else
        {
            json = File.ReadAllText(filePath);
        }

        songData = JsonUtility.FromJson<SongData>(json);
        GetDataFromJson();
    }

    private void GetDataFromJson()
    {
        foreach (var lane in lanes)
        {
            var noteData = songData.notes.Find(note => note.noteRestriction == lane.noteRestriction);
            if (noteData != null)
            {
                lane.SetTimeStamps(noteData.timeStamps);
            }
        }

        Invoke(nameof(StartSong), songDelayInSeconds);
    }

    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }

    public void StartSong()
    {
        audioSource.Play();
        Invoke(nameof(EndSong), audioSource.clip.length);
    }

    public void StopSong()
    {
        StartCoroutine(FadeOutAndStop());
    }

    public void Pause()
    {
        audioSource.Pause();
        currentResumeTimer = SongResumeTimer;
    }

    public void Resume()
    {
        StartCoroutine(ResumeSong());
    }

    private IEnumerator ResumeSong()
    {
        yield return new WaitForSeconds(SongResumeTimer);
        audioSource.Play();

        while (currentResumeTimer > 0)
        {
            yield return new WaitForSeconds(1);
            currentResumeTimer--;
        }
    }

    public void Failed()
    {
        isPause = true;
        StartCoroutine(FadeOutAndStop());
    }

    private IEnumerator FadeOutAndStop()
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < stopSongFadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / stopSongFadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // Reset volume to start volume for the next audio clip

        // Play the fail sound effect at the original volume
        if (songFailSFX != null)
        {
            audioSource.clip = songFailSFX;
            audioSource.Play();
        }
    }

    private void EndSong()
    {
        OnSongEnd?.Invoke();
    }
}

public enum NoteDirection { Y, X, Z };
