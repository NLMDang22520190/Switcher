using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTapVisual : MonoBehaviour
{
    [SerializeField] private float attackLifeTime;
    private NormalPlayerTap playerTap;

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
        yield return new WaitForSeconds(attackLifeTime);
        Return();
    }

    private void Return()
    {
        gameObject.SetActive(false);
        playerTap.ReturnTapVisual(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<NormalEnemy>(out NormalEnemy enemy))
        {
            enemy.CurrentSpeed = 0;
            enemy.GetComponent<Collider>().enabled = false;

            if (NormalSpawnPoint.Instance != null)
            {
                NormalSpawnPoint.Instance.ReturnEnemy(enemy);
            }

            if (playerTap != null)
            {
                playerTap.GetHitVisual(transform.position);
                playerTap.ReturnTapVisual(this);
            }

            if (NormalScoreManager.Instance != null)
            {
                NormalScoreManager.Instance.AddScore(enemy.ScoreValue);
                NormalScoreManager.Instance.AddCombo();
            }
        }   
    }
}
