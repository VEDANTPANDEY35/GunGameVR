using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public int maxEnemies = 5;
    public float spawnDelay = 2f;

    private int currentEnemies = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnDelay);
    }

    void SpawnEnemy()
    {
        if (currentEnemies >= maxEnemies) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        currentEnemies++;

        // 🔥 IMPORTANT: decrease count when enemy dies
        enemy.GetComponent<EnemyHealth>().onDeath += () =>
        {
            currentEnemies--;
        };
    }
}