using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnComplete : MonoBehaviour
{
    [SerializeField] private AudioSource BgmSource;

    private void Start()
    {
        BgmSource.Stop();
    }

}
