using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCoin : MonoBehaviour
{
    [SerializeField] private int scoreValue = 100;
    [SerializeField] private Transform coinMesh;
    [SerializeField] private Transform castPoint;
    [SerializeField] private Transform coinCenter;

    private void Update()
    {
        RotateMesh();
    }

    private void RotateMesh()
    {
        float rotationSpeed = 50f;
        coinMesh.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            if (MusicScoreManager.Instance != null)
            {
                MusicScoreManager.Instance.AddScore(scoreValue);
                MusicScoreManager.Instance.AddCombo();
                Destroy(gameObject);
            }
        }
    }
}
