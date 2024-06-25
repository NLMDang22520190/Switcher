using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalHitVisual : MonoBehaviour
{
    private NormalPlayerTap playerTap;

    private int hitLifeTime = 1;

    private void Start()
    {
        playerTap = NormalPlayerTap.Instance;
    }

    private void OnEnable()
    {
        StartCoroutine(ReturnAfterLifetime());
    }

    private IEnumerator ReturnAfterLifetime()
    {
        yield return new WaitForSeconds(hitLifeTime);
        Return();
    }

    private void Return()
    {
        gameObject.SetActive(false);
        playerTap.ReturnHitVisual(this);
    }
}
