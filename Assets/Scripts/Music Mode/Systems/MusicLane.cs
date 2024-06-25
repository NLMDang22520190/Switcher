using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Melanchall.DryWetMidi.Interaction;
using UnityEditor;
using UnityEngine.SceneManagement;

public class MusicLane : MonoBehaviour
{
    public string noteRestriction; // Changed from Melanchall.DryWetMidi.MusicTheory.NoteName to string
    public GameObject notePrefab;
    private List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();
    public int scoreValue;

    public bool isTopLane;
    public bool isSpawningEnemy;
    public LayerMask groundLayer;
    public Transform noteSpawnPoint;

    private PlayerInput inputs;

    private int spawnIndex = 0;
    private int inputIndex = 0;

    private void Start()
    {
        inputs = Player.Instance.GetComponent<PlayerInput>();
    }

    public void SetTimeStamps(List<double> timeStamps)
    {
        this.timeStamps = timeStamps;
    }

    private void Update()
    {
        Switching();
    }

    private void Switching()
    {
        Vector3 castDirection = isTopLane ? Vector3.down : Vector3.up;
        float castHeight = 10f;
        Vector3 castPosition = isTopLane ? transform.position + castHeight * Vector3.up : transform.position + castHeight * Vector3.down;

        RaycastHit hit;
        Physics.Raycast(castPosition, castDirection, out hit, 999f, groundLayer);
        noteSpawnPoint.position = hit.point;

        if (spawnIndex < timeStamps.Count && !SongManager.Instance.isPause)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
            {
                var note = Instantiate(notePrefab, noteSpawnPoint);
                if (!isTopLane)
                {
                    note.transform.Rotate(Vector3.right, 180);
                }

                notes.Add(note.GetComponent<Note>());

                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                note.GetComponent<Note>().isTopNote = isTopLane;
                spawnIndex++;
            }
        }

        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            double nearMarginOfError = SongManager.Instance.nearMarginOfError; // Near margin of error
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if (isSpawningEnemy)
            {
                if (inputs.Tap())
                {
                    if (Math.Abs(audioTime - timeStamp) < marginOfError)
                    {
                        Hit(scoreValue); // Full score
                        print($"Hit on {inputIndex} note");
                        MusicScoreManager.Instance.PerfectIndicator.text = "Perfect!!!";
                        MusicScoreManager.Instance.PerfectIndicator.color = Color.magenta;
                        Destroy(notes[inputIndex].gameObject);
                        inputIndex++;
                    }
                    else if (Math.Abs(audioTime - timeStamp) < nearMarginOfError)
                    {
                        Hit(scoreValue * 2 / 3); // Partial score
                        print($"Near hit on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                        MusicScoreManager.Instance.PerfectIndicator.text = "Good!";
                        MusicScoreManager.Instance.PerfectIndicator.color = Color.cyan;
                        Destroy(notes[inputIndex].gameObject);
                        inputIndex++;
                    }
                    else
                    {
                        print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                    }
                }
                if (timeStamp + marginOfError <= audioTime)
                {
                    Miss();
                    print($"Missed {inputIndex} note");
                    MusicScoreManager.Instance.PerfectIndicator.text = "Missed!";
                    MusicScoreManager.Instance.PerfectIndicator.color = Color.red;
                    inputIndex++;
                }
            }
        }
    }

    private void Hit(int score)
    {
        MusicScoreManager.Instance.AddScore(score);
        MusicScoreManager.Instance.AddCombo();

        Transform attackPoint = Player.Instance.transform.Find("AttackPoint");
        if (attackPoint != null)
        {
            MusicPlayerTap.Instance.GetHitVisual(attackPoint.position);
        }
        else
        {
            Debug.LogError("Attack Point not found on Player.");
        }

        // return the latest tap visual
        MusicPlayerTap.Instance.ReturnLatestTapVisual();
    }

    private void Miss()
    {
        MusicScoreManager.Instance.ResetCombo();
    }
}
