using UnityEngine;

public class MusicMainLane : MonoBehaviour
{
    public static MusicMainLane Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        AttachToPlayer();
    }

    private void AttachToPlayer()
    {
        this.transform.position = Player.Instance.transform.position;
    }
}
