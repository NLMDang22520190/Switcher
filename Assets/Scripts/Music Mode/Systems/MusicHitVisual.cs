using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHitVisual : MonoBehaviour
{
    private MusicPlayerTap playerTap;


    private void Start()
    {
        playerTap = MusicPlayerTap.Instance;
    }

    private void OnEnable()
    {
        StartCoroutine(ReturnAfterLifetime());
    }

    private IEnumerator ReturnAfterLifetime()
    {
        yield return new WaitForSeconds(1);
        Return();
    }

    private void Return()
    {
        gameObject.SetActive(false);
        playerTap.ReturnHitVisual(this);
    }
}
