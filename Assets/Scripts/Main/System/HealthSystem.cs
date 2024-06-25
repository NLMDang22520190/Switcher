using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    public int currentHealth { get; private set; }

    public bool isDead { get { return currentHealth <= 0; } }

    // Define a death event
    public event Action OnDeath;

    // Define a health change event
    public event Action<int> OnHealthChanged;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // Trigger the health change event
        OnHealthChanged?.Invoke(currentHealth);

        if (isDead)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Trigger the health change event
        OnHealthChanged?.Invoke(currentHealth);
    }

    private void Die()
    {
        // Trigger the death event
        OnDeath?.Invoke();
        gameObject.GetComponent<Collider>().enabled = false;
    }

    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }
}
