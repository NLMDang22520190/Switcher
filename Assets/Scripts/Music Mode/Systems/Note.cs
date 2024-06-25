using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    private double timeInstantiated;
    public float assignedTime;

    public bool isTopNote;
    Vector3 noteDirection;


    void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
        switch (SongManager.Instance.noteDirection)
        {
            case NoteDirection.X:
                noteDirection = Vector3.right;
                break;
            case NoteDirection.Y:
                noteDirection = Vector3.up;
                break;
            case NoteDirection.Z:
                noteDirection = Vector3.forward;
                break;
        }

    }

    void Update()
    {
        NoteMovement();

    }

    private void LateUpdate()
    {
        MakeNoteOnGround();
    }

    private void NoteMovement()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));

        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(noteDirection * SongManager.Instance.noteSpawn, noteDirection * SongManager.Instance.noteDespawnY, t);
        }
    }

    private void MakeNoteOnGround()
    {
        Vector3 castDirection = isTopNote ? Vector3.down : Vector3.up;
        float castHeight = 10f;
        Vector3 castPosition = isTopNote ? transform.position + castHeight * Vector3.up : transform.position + castHeight * Vector3.down;

        RaycastHit hit;
        Physics.Raycast(castPosition, castDirection, out hit, 999f, groundLayer);
        transform.position = hit.point;
    }
}
