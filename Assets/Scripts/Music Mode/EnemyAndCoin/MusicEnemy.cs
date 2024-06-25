using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            HealthSystem playerHealth = other.GetComponent<HealthSystem>();
            playerHealth.TakeDamage(1);
        }
    }
}
