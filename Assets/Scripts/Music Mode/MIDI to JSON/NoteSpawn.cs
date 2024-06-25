using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawn : MonoBehaviour
{
    private double timeInstantiated;
    public float assignedTime;

    void Start()
    {
        timeInstantiated = MidiToJsonConverter.GetAudioSourceTime();
    }

    void Update()
    {
        double timeSinceInstantiated = MidiToJsonConverter.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (MidiToJsonConverter.Instance.noteTime * 2));

        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.up * MidiToJsonConverter.Instance.noteSpawnX, Vector3.up * MidiToJsonConverter.Instance.noteDespawnX, t);
        }
    }
}
