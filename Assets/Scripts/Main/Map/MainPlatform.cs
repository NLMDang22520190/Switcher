using System.Collections.Generic;
using UnityEngine;

public class MainPlatform : MonoBehaviour
{
    private const float PLAYER_TO_LAST_ENDPOINT_DISTANCE = 100f;

    [SerializeField] private Platform[] lanePrefabs;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Player player;

    private Transform lastEndPoint;
    private List<Platform> laneList = new List<Platform>();

    private void Awake()
    {
        lastEndPoint = endPoint;
        CreateLanes();
    }

    private void LateUpdate()
    {
        if (GetDistance() < PLAYER_TO_LAST_ENDPOINT_DISTANCE)
        {
            ActivateNextLane();
        }
    }

    private void CreateLanes()
    {
        foreach (Platform lanePrefab in lanePrefabs)
        {
            for (int i = 0; i < 5; i++)
            {
                Platform laneInstance = Instantiate(lanePrefab, Vector3.zero, Quaternion.identity);
                laneInstance.gameObject.SetActive(false);
                laneList.Add(laneInstance);
            }
        }
    }

    private void ActivateNextLane()
    {
        foreach (Platform lane in laneList)
        {
            if (!lane.gameObject.activeSelf)
            {
                ActivateLane(lane, lastEndPoint.position);
                return;
            }
        }

        CreateLanes();
        ActivateNextLane();
    }

    private void ActivateLane(Platform lane, Vector3 spawnPosition)
    {
        lane.transform.position = spawnPosition;
        lane.gameObject.SetActive(true);
        lastEndPoint = lane.EndPoint;
    }

    private float GetDistance()
    {
        return Vector3.Distance(player.transform.position, lastEndPoint.position);
    }
}
