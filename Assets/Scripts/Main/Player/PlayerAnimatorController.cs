using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private const string DIE = "Die";


    [SerializeField] private Animator animator;

    private Player player;
    private HealthSystem healthSystem;


    private void Start()
    {
        player = Player.Instance;
        healthSystem = player.GetComponent<HealthSystem>();

        // subscribe to the death event
        healthSystem.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        animator.Play(DIE);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void OnDestroy()
    {
        healthSystem.OnDeath -= OnDeath;
    }
}
