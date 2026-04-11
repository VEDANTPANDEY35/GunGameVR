using UnityEngine;

public class HeadshotTarget : MonoBehaviour
{
    public int headshotMultiplier = 2;

    public void TakeHeadshot(int baseDamage)
    {
        EnemyHealth enemy = GetComponentInParent<EnemyHealth>();

        if (enemy != null)
        {
            int finalDamage = baseDamage * headshotMultiplier;
            enemy.TakeDamage(finalDamage);

            Debug.Log("HEADSHOT!");
        }
    }
}