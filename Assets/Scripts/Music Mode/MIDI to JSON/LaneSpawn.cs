using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Interaction;
using System;

public class LaneSpawn : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public GameObject enemyPrefab;
    List<NoteSpawn> enemies = new List<NoteSpawn>();
    public List<double> timeStamps = new List<double>();
    public KeyCode[] inputs;
    public int scoreAmount;
    public bool isSpawningEnemy = false;

    int spawnIndex = 0;

    private void Awake()
    {
    }

    private void Update()
    {
        SetIndex();
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, MidiToJsonConverter.midiFile.GetTempoMap());
                double timeStamp = (double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f;
                timeStamps.Add(timeStamp);
            }
        }

        NoteData noteData = new NoteData
        {
            noteRestriction = noteRestriction.ToString(),
            timeStamps = new List<double>(timeStamps)
        };
        MidiToJsonConverter.Instance.songData.notes.Add(noteData);
    }

    private void SetIndex()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if (MidiToJsonConverter.Instance.isSongPlaying)
            {
                if (MidiToJsonConverter.GetAudioSourceTime() >= timeStamps[spawnIndex] - MidiToJsonConverter.Instance.noteTime)
                {
                    var enemy = Instantiate(enemyPrefab, transform);
                    enemies.Add(enemy.GetComponent<NoteSpawn>());
                    enemy.GetComponent<NoteSpawn>().assignedTime = (float)timeStamps[spawnIndex];
                    spawnIndex++;
                }
            }
        }
    }
}
