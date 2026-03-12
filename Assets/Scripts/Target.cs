using UnityEngine;

public class Target : MonoBehaviour
{
    public int health = 150;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Health remaining: " + health);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}