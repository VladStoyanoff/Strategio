using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    [SerializeField] int health = 100;
    int healthMax;

    public event EventHandler OnDamaged;

    void Awake()
    {
        healthMax = health;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        OnDamaged?.Invoke(this, EventArgs.Empty);
        Debug.Log(health);
        if (health > 0) return;
        Die();
        health = 0;
    }

    void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized() => (float)health / healthMax;
}
