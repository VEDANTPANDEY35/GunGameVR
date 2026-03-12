using UnityEngine;

public class HeadshotTarget : MonoBehaviour
{
    public Target bodyTarget;
    public int headshotMultiplier = 4;

    public void TakeHeadshot(int damage)
    {
        if (bodyTarget != null)
        {
            bodyTarget.TakeDamage(damage * headshotMultiplier);
        }
    }
}