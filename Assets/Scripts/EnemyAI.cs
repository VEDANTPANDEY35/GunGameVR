using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float attackDistance = 2f;
    public int damage = 10;

    private NavMeshAgent agent;
    private Animator animator;

    float attackCooldown = 1f;
    float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>(); // 🔥 important for animation
    }

    void Update()
    {
        // 🔥 AUTO-FIND PLAYER (no manual drag needed)
        if (player == null)
        {
            GameObject xr = GameObject.Find("XR Origin (XR Rig)");
            if (xr != null)
                player = xr.transform;
            else
                return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        // ✅ MOVE using NavMesh
        agent.SetDestination(player.position);

        // 🎬 ANIMATION CONTROL
        if (animator != null)
        {
            float speed = agent.velocity.magnitude;
            animator.SetFloat("Speed", speed); // drives walk animation
        }

        // ⚔ ATTACK
        if (distance <= attackDistance && Time.time > lastAttackTime + attackCooldown)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
            }

            lastAttackTime = Time.time;
        }
    }
}