using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    public int damage = 40;

    void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
}