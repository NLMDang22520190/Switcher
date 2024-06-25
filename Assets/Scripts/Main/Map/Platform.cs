using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Transform endPoint;

    private Player player;
    private bool isPlayerOnLane;
    private bool deactivationScheduled;

    private void Start()
    {
        player = Player.Instance;
    }

    public Transform EndPoint => endPoint;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            isPlayerOnLane = true;
            deactivationScheduled = false;
            CancelInvoke(nameof(DeactivateLane));  // Cancel any pending deactivation
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            isPlayerOnLane = false;
            if (!deactivationScheduled)
            {
                deactivationScheduled = true;
                Invoke(nameof(DeactivateLane), 3f);  // Schedule deactivation after 3 seconds
            }
        }
    }

    private void DeactivateLane()
    {
        if (!isPlayerOnLane)
        {
            gameObject.SetActive(false);
        }
        deactivationScheduled = false;  // Reset the flag after deactivation
    }
}
