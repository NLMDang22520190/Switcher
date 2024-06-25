using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMissDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<NormalEnemy>(out NormalEnemy enemy))
        {
            if (NormalScoreManager.Instance != null)
            {
                NormalScoreManager.Instance.ResetCombo();
            }
        }
    }
}
