using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGameManager : MonoBehaviour
{
    private static NormalGameManager instance;
    public static NormalGameManager Instance { get => instance; }

    private NormalSpawnPoint spawnPoint;
    private Player player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        // Subscribe to the score changed event
        if (NormalScoreManager.Instance != null)
        {
            NormalScoreManager.Instance.OnScoreChanged += HandleScoreChanged;
        }

        player = Player.Instance;
        spawnPoint = NormalSpawnPoint.Instance;
    }

    private void HandleScoreChanged(int score)
    {
        int multiplier = score / 500;
        // Increase the player's speed by 20% for every 500 points
        if (player.CurrentSpeed < 30)   // limit the speed to 23
        {
            player.CurrentSpeed = player.Speed * (multiplier * 0.2f + 1);
        }
        else
        {
            player.CurrentSpeed = 30;
        }

        // Increase the spawn distance by 10% for every 500 points
        if (spawnPoint.CurrentSpawnPointsDistance < 5)  // limit the spawn distance to 5
        {
            NormalSpawnPoint.Instance.CurrentSpawnPointsDistance = NormalSpawnPoint.Instance.SpawnPointsDistance * (multiplier * 0.1f + 1);
        }
        else
        {
            NormalSpawnPoint.Instance.CurrentSpawnPointsDistance = 5;
        }

        // reduce the spawn rate by 10% for every 500 points
        if (spawnPoint.CurrentSpawnRate > 1f)   // limit the spawn rate to 1
        {
            NormalSpawnPoint.Instance.CurrentSpawnRate = NormalSpawnPoint.Instance.SpawnRate * (1 - multiplier * 0.1f);
        }
        else
        {
            NormalSpawnPoint.Instance.CurrentSpawnRate = 1f;
        }

        Debug.Log($"Player speed updated: {Player.Instance.CurrentSpeed}");
        Debug.Log($"Spawn points distance updated: {NormalSpawnPoint.Instance.CurrentSpawnPointsDistance}");
        Debug.Log($"Spawn rate updated: {NormalSpawnPoint.Instance.CurrentSpawnRate}");
    }

    private void OnDestroy()
    {
        // Unsubscribe from the score changed event
        if (NormalScoreManager.Instance != null)
        {
            NormalScoreManager.Instance.OnScoreChanged -= HandleScoreChanged;
        }
    }
}
