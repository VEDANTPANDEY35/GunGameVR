using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 50;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameManager.instance.AddScore(1);
        Destroy(gameObject);
    }
}