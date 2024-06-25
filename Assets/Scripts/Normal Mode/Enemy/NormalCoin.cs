using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCoin : MonoBehaviour
{
    [SerializeField] private int scoreValue = 100;
    [SerializeField] private Transform coinMesh;
    [SerializeField] private Transform castPoint;
    [SerializeField] private Transform coinCenter;

    private NormalSpawnPoint normalMapLane;
    private LayerMask groundLayer;

    private void Start()
    {
        normalMapLane = NormalSpawnPoint.Instance;
        groundLayer = normalMapLane.GroundLayer;

        int coinLifeTime = 5;
        Invoke(nameof(ReturnObject), coinLifeTime);
    }

    private void LateUpdate()
    {
        Move();
        RotateMesh();
    }

    private void Move()
    {
        if (Physics.Raycast(castPoint.position, castPoint.position - transform.position, out RaycastHit hit, 10f, groundLayer))
        {
            transform.position = hit.point;
        }
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
            if (NormalScoreManager.Instance != null)
            {
                NormalScoreManager.Instance.AddScore(scoreValue);
                NormalScoreManager.Instance.AddCombo();
            }

            normalMapLane.GetCoinParticle(coinCenter.position);
            ReturnObject();
        }
    }

    private void ReturnObject()
    {
        normalMapLane.ReturnCoin(this);
        gameObject.SetActive(false);
    }
}
