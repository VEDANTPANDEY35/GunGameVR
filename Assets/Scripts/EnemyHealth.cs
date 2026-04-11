using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 100;

    public Action onDeath;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            onDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}