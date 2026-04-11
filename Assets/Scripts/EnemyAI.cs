using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // AUTO FIND PLAYER
        player = Camera.main.transform;
    }

    void Update()
    {
        if (player == null) return;

        agent.SetDestination(player.position);
    }
}